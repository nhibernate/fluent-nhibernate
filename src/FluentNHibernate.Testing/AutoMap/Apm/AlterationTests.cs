using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.AutoMap.TestFixtures;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.AutoMap.Apm
{
    [TestFixture]
    public class AlterationTests
    {
        private AutoPersistenceModel model;

        [SetUp]
        public void CreateAutoMapper()
        {
            model = AutoPersistenceModel.MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == typeof(ExampleClass).Namespace);
        }

        [Test]
        public void ShouldApplyAlterationsToModel()
        {
            var alteration = MockRepository.GenerateMock<IAutoMappingAlteration>();

            model
                .Alterations(alterations => alterations.Add(alteration))
                .CompileMappings();

            alteration.AssertWasCalled(x => x.Alter(model));
        }

        [Test]
        public void UseOverridesAddsAlteration()
        {
            model.UseOverridesFromAssemblyOf<ExampleClass>()
                .Alterations(alterations =>
                    alterations.ShouldContain(a => a is AutoMappingOverrideAlteration));
        }
    }
}
