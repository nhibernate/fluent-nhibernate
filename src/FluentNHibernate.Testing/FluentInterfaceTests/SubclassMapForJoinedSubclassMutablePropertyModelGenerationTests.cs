using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class SubclassMapForJoinedSubclassMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AbstractShouldSetModelAbstractPropertyToTrue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void NotAbstractShouldSetModelAbstractPropertyToFalse()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeFalse());
        }

        [Test]
        public void DynamicInsertShouldSetModelDynamicInsertPropertyToTrue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicInsertShouldSetModelDynamicInsertPropertyToFalse()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeFalse());
        }

        [Test]
        public void DynamicUpdateShouldSetModelDynamicUpdatePropertyToTrue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicUpdateShouldSetModelDynamicUpdatePropertyToFalse()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeFalse());
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(false));
        }

        [Test]
        public void ProxyGenericShouldSetModelProxyPropertyToType()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Proxy<FakeProxyType>())
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void ProxyShouldSetModelProxyPropertyToType()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Proxy(typeof(FakeProxyType)))
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void SelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToTrue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotSelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToFalse()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeFalse());
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Table("table"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("table"));
        }

        [Test]
        public void TableNameShouldDefaultToEntityName()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => {})
                .ModelShouldMatch(x => x.TableName.ShouldEqual("`ChildRecord`"));
        }

        [Test]
        public void KeyColumnShouldSetModelKeyColumnNamePropertyToValue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.KeyColumn("column"))
                .ModelShouldMatch(x => x.Key.Columns.First().Name.ShouldEqual("column"));
        }

        [Test]
        public void KeyColumnShouldDefaultToEntityName()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Key.Columns.First().Name.ShouldEqual("SuperRecord_id"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Schema("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }

        [Test]
        public void CheckConstraintShouldSetModelCheckPropertyToValue()
        {
            SubclassMapForJoinedSubclass<ChildRecord>()
                .Mapping(m => m.Check("constraint"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("constraint"));
        }

        [Test]
        public void DiscriminatorValueShouldntBreakEvenThoughItIsntSupported()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.DiscriminatorValue("value"))
                .ModelShouldMatch(x => { });
        }

        private class FakeProxyType
        {}
    }
}