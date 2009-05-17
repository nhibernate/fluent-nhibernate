using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
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
        private Configuration cfg;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            model = new PersistenceModel(conventionFinder);

            cfg = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .BuildConfiguration();
        }

        //[Test]
        //public void ShouldFindConventions()
        //{
        //    model.Configure(cfg);
            
        //    conventionFinder.AssertWasCalled(x => x.Find<IEntireMappingsConvention>());
        //}

        //[Test]
        //public void ShouldCheckIfConventionsWillAcceptTheMappings()
        //{
        //    var convention = MockRepository.GenerateMock<IEntireMappingsConvention>();
            
        //    conventionFinder.Stub(x => x.Find<IEntireMappingsConvention>())
        //        .Return(new[] { convention });

        //    model.Configure(cfg);

        //    convention.AssertWasCalled(x => x.Accept(null),
        //        constraints => constraints.IgnoreArguments());
        //}

        //[Test]
        //public void ShouldApplyConventionsIfAcceptIsTrue()
        //{
        //    var convention = MockRepository.GenerateMock<IEntireMappingsConvention>();

        //    conventionFinder.Stub(x => x.Find<IEntireMappingsConvention>())
        //        .Return(new[] { convention });

        //    convention.Stub(x => x.Accept(null))
        //        .IgnoreArguments()
        //        .Return(true);

        //    model.Configure(cfg);

        //    convention.AssertWasCalled(x => x.Apply(null),
        //        constraints => constraints.IgnoreArguments());
        //}

        //[Test]
        //public void ShouldntApplyConventionsIfAcceptIsFalse()
        //{
        //    var convention = MockRepository.GenerateMock<IEntireMappingsConvention>();

        //    conventionFinder.Stub(x => x.Find<IEntireMappingsConvention>())
        //        .Return(new[] { convention });

        //    convention.Stub(x => x.Accept(null))
        //        .IgnoreArguments()
        //        .Return(false);

        //    model.Configure(cfg);

        //    convention.AssertWasNotCalled(x => x.Apply(null),
        //        constraints => constraints.IgnoreArguments());
        //}
    }
}