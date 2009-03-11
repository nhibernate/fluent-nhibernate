using FluentNHibernate.Conventions;
using NHibernate.Cfg;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.PersistenceModelTests.Conventions
{
    [TestFixture]
    public class Configure
    {
        private PersistenceModel model;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            model = new PersistenceModel(conventionFinder);
        }

        [Test]
        public void ShouldFindConventions()
        {
            model.Configure(new Configuration());
            
            conventionFinder.AssertWasCalled(x => x.Find<IAssemblyConvention>());
        }

        [Test]
        public void ShouldCheckIfConventionsWillAcceptTheMappings()
        {
            var convention = MockRepository.GenerateMock<IAssemblyConvention>();
            
            conventionFinder.Stub(x => x.Find<IAssemblyConvention>())
                .Return(new[] { convention });

            model.Configure(new Configuration());

            convention.AssertWasCalled(x => x.Accept(null),
                constraints => constraints.IgnoreArguments());
        }

        [Test]
        public void ShouldApplyConventionsIfAcceptIsTrue()
        {
            var convention = MockRepository.GenerateMock<IAssemblyConvention>();

            conventionFinder.Stub(x => x.Find<IAssemblyConvention>())
                .Return(new[] { convention });

            convention.Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(true);

            model.Configure(new Configuration());

            convention.AssertWasCalled(x => x.Apply(null, null),
                constraints => constraints.IgnoreArguments());
        }

        [Test]
        public void ShouldntApplyConventionsIfAcceptIsFalse()
        {
            var convention = MockRepository.GenerateMock<IAssemblyConvention>();

            conventionFinder.Stub(x => x.Find<IAssemblyConvention>())
                .Return(new[] { convention });

            convention.Stub(x => x.Accept(null))
                .IgnoreArguments()
                .Return(false);

            model.Configure(new Configuration());

            convention.AssertWasNotCalled(x => x.Apply(null, null),
                constraints => constraints.IgnoreArguments());
        }
    }
}