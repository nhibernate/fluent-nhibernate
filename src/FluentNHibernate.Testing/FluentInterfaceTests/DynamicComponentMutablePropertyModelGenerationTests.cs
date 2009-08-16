using System.Collections;
using System.Linq;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class DynamicComponentSubPartTests : BaseModelFixture
    {
        [Test]
        public void ShouldGeneratePropertyMapUsingString()
        {
            DynamicComponent<IDictionary>()
                .Mapping(x => x.Map("name"))
                .ModelShouldMatch(x =>
                {
                    x.Properties.Count().ShouldEqual(1);
                    x.Properties.First().Name.ShouldEqual("name");
                });
        }

        [Test]
        public void ShouldGeneratePropertyMapUsingStringWithDefaultTypeOfString()
        {
            DynamicComponent<IDictionary>()
                .Mapping(x => x.Map("name"))
                .ModelShouldMatch(x =>
                {
                    x.Properties.First().Type.GetUnderlyingSystemType().ShouldEqual(typeof(string));
                });
        }

        [Test]
        public void ShouldGeneratePropertyMapUsingStringWithExplicitType()
        {
            DynamicComponent<IDictionary>()
                .Mapping(x => x.Map<int>("name"))
                .ModelShouldMatch(x =>
                {
                    x.Properties.First().Type.GetUnderlyingSystemType().ShouldEqual(typeof(int));
                });
        }

        [Test]
        public void ShouldGeneratePropertyMap()
        {
            DynamicComponent<IDictionary>()
                .Mapping(m => m.Map(x => x["name"]))
                .ModelShouldMatch(x =>
                {
                    x.Properties.Count().ShouldEqual(1);
                    x.Properties.First().Name.ShouldEqual("name");
                });
        }
    }
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