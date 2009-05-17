using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Defaults;
//using FluentNHibernate.Conventions.Discovery;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class OnConstruction
    {
        private IConventionFinder conventionFinder;
        private PersistenceModel model;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            model = new PersistenceModel(conventionFinder);
        }

        //[Test]
        //public void ConstructorShouldAddDiscoveryConventionsToFinder()
        //{
        //    conventionFinder.AssertWasCalled(x => x.Add(typeof(ClassDiscoveryConvention)));
        //    conventionFinder.AssertWasCalled(x => x.Add(typeof(IdDiscoveryConvention)));
        //}

        [Test]
        public void ConstructorShouldntAddDefaultConventionsToFinder()
        {
            conventionFinder.AssertWasNotCalled(x => x.Add(typeof(TableNameConvention)));
            conventionFinder.AssertWasNotCalled(x => x.Add(typeof(PrimaryKeyConvention)));
        }

        //[Test]
        //public void ApplyShouldAddDefaultConventionsToFinder()
        //{
        //    model.ApplyConventions();

        //    conventionFinder.AssertWasCalled(x => x.Add(typeof(TableNameConvention)));
        //    conventionFinder.AssertWasCalled(x => x.Add(typeof(PrimaryKeyConvention)));
        //}
    }
}