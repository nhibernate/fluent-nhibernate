using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ComponentMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void InsertShouldSetModelPropertyToTrue()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NotInsertShouldSetModelPropertyToFalse()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.Not.Insert())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void UpdateShouldSetModelPropertyToTrue()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }

        [Test]
        public void NotUpdateShouldSetModelPropertyToFalse()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.Not.Update())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetModelInsertPropertyToFalse()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetModelUpdatePropertyToFalse()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetModelInsertPropertyToTrue()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.Not.ReadOnly())
                .ModelShouldMatch(x => x.Insert.ShouldBeTrue());
        }

        [Test]
        public void NotReadOnlyShouldSetModelUpdatePropertyToTrue()
        {
            Component<PropertyTarget>()
                .Mapping(x => x.Not.ReadOnly())
                .ModelShouldMatch(x => x.Update.ShouldBeTrue());
        }
    }
}