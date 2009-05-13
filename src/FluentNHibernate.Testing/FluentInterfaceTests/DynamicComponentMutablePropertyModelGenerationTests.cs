using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class DynamicComponentMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void InsertShouldSetModelPropertyToTrue()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NotInsertShouldSetModelPropertyToFalse()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.Not.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void UpdateShouldSetModelPropertyToTrue()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void NotUpdateShouldSetModelPropertyToFalse()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.Not.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetModelInsertPropertyToFalse()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetModelUpdatePropertyToFalse()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetModelInsertPropertyToTrue()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.Not.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NotReadOnlyShouldSetModelUpdatePropertyToTrue()
        {
            DynamicComponent<PropertyTarget>()
                .Mapping(x => x.Not.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }
    }
}