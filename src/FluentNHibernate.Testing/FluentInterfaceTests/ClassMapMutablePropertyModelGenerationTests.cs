using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ClassMapMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void LazyLoadSetsModelPropertyToTrue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.LazyLoad())
                .ModelShouldMatch(x => x.LazyLoad.ShouldBeTrue());
        }

        [Test]
        public void NotLazyLoadSetsModelPropertyToFalse()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Not.LazyLoad())
                .ModelShouldMatch(x => x.LazyLoad.ShouldBeFalse());
        }

        [Test]
        public void WithTableShouldSetModelPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.WithTable("table"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("table"));
        }

        [Test]
        public void DynamicInsertShouldSetModelPropertyToTrue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicInsertShouldSetModelPropertyToFalse()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Not.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeFalse());
        }

        [Test]
        public void DynamicUpdateShouldSetModelPropertyToTrue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicUpdateShouldSetModelPropertyToFalse()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Not.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetModelMutablePropertyToFalse()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.ReadOnly())
                .ModelShouldMatch(x => x.Mutable.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetModelMutablePropertyToTrue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Not.ReadOnly())
                .ModelShouldMatch(x => x.Mutable.ShouldBeTrue());
        }

        [Test]
        public void SchemaIsShouldSetModelPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.SchemaIs("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }
    }
}
