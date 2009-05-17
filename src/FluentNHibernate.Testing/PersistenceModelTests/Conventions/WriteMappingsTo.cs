using System.IO;
using FluentNHibernate.Conventions;
using NHibernate.Cfg;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.PersistenceModelTests.Conventions
{
    [TestFixture]
    public class WriteMappingsTo
    {
        private PersistenceModel model;
        private IConventionFinder conventionFinder;
        private string TempDir;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            model = new PersistenceModel(conventionFinder);

            TempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            RemoveArtifacts();
            Directory.CreateDirectory(TempDir);
        }

        [TearDown]
        public void RemoveArtifacts()
        {
            if (Directory.Exists(TempDir))
                Directory.Delete(TempDir, true);
        }

        //[Test]
        //public void ShouldFindConventions()
        //{
        //    model.WriteMappingsTo(TempDir);

        //    conventionFinder.AssertWasCalled(x => x.Find<IEntireMappingsConvention>());
        //}

        //[Test]
        //public void ShouldCheckIfConventionsWillAcceptTheMappings()
        //{
        //    var convention = MockRepository.GenerateMock<IEntireMappingsConvention>();

        //    conventionFinder.Stub(x => x.Find<IEntireMappingsConvention>())
        //        .Return(new[] { convention });

        //    model.WriteMappingsTo(TempDir);

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

        //    model.WriteMappingsTo(TempDir);

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

        //    model.WriteMappingsTo(TempDir);

        //    convention.AssertWasNotCalled(x => x.Apply(null),
        //        constraints => constraints.IgnoreArguments());
        //}
    }
}