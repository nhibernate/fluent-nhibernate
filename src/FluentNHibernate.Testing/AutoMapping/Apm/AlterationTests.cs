using FakeItEasy;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.TestFixtures;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping.Apm
{
    [TestFixture]
    public class AlterationTests
    {
        private AutoPersistenceModel model;

        [SetUp]
        public void CreateAutoMapper()
        {
            model = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == typeof(ExampleClass).Namespace);
        }

        [Test]
        public void ShouldApplyAlterationsToModel()
        {
            var alteration = A.Fake<IAutoMappingAlteration>();

            model
                .Alterations(alterations => alterations.Add(alteration))
                .BuildMappings();

            A.CallTo(() => alteration.Alter(model)).MustHaveHappened();
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
