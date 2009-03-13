using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class SubclassDiscoveryConventionTester
    {
        private SubclassDiscoveryConvention convention;
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreateConvention()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            convention = new SubclassDiscoveryConvention(conventionFinder);
        }

        [Test]
        public void ShouldAcceptISubclasses()
        {
            convention.Accept(MockRepository.GenerateStub<ISubclass>())
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldntAcceptAnyOtherIMappingParts()
        {
            convention.Accept(MockRepository.GenerateStub<IProperty>())
                .ShouldBeFalse();
        }

        [Test]
        public void ApplyFindsConventions()
        {
            conventionFinder.Stub(x => x.Find<ISubclassConvention>())
                .Return(new ISubclassConvention[] { });

            convention.Apply(MockRepository.GenerateStub<ISubclass>());
            conventionFinder.AssertWasCalled(x => x.Find<ISubclassConvention>());
        }

        [Test]
        public void ApplyCallsAcceptOnAllConventionsAgainstEachClass()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<ISubclassConvention>(),
                MockRepository.GenerateMock<ISubclassConvention>()
            };
            var id = MockRepository.GenerateStub<ISubclass>();

            conventionFinder.Stub(x => x.Find<ISubclassConvention>())
                .Return(conventions);

            convention.Apply(id);

            conventions[0].AssertWasCalled(x => x.Accept(id));
            conventions[1].AssertWasCalled(x => x.Accept(id));
        }

        [Test]
        public void ApplyAppliesAllAcceptedConventions()
        {
            var conventions = new[]
            {
                MockRepository.GenerateMock<ISubclassConvention>(),
                MockRepository.GenerateMock<ISubclassConvention>()
            };
            var id = MockRepository.GenerateStub<ISubclass>();

            conventionFinder.Stub(x => x.Find<ISubclassConvention>())
                .Return(conventions);

            conventions[0].Stub(x => x.Accept(id)).Return(true);
            conventions[1].Stub(x => x.Accept(id)).Return(false);

            convention.Apply(id);

            // each convention gets Apply called for any properties it returned true for Accept
            conventions[0].AssertWasCalled(x => x.Apply(id));
            conventions[1].AssertWasNotCalled(x => x.Apply(id));
        }
    }
}