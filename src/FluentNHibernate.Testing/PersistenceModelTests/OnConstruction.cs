using FluentNHibernate.Conventions;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class OnConstruction
    {
        private IConventionFinder conventionFinder;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventionFinder = MockRepository.GenerateMock<IConventionFinder>();
            new PersistenceModel(conventionFinder);
        }

        [Test]
        public void ConstructorShouldAddFNHAssemblyToFinder()
        {
            var fluentNHibernateAssembly = typeof(PersistenceModel).Assembly;

            conventionFinder.AssertWasCalled(x => x.AddAssembly(fluentNHibernateAssembly));
        }
    }
}