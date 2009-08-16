using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class SubclassMapForSubclassMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AbstractShouldSetModelAbstractPropertyToTrue()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void NotAbstractShouldSetModelAbstractPropertyToFalse()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Not.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeFalse());
        }

        [Test]
        public void DiscriminatorValueShouldSetModelDiscriminatorValue()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.DiscriminatorValue("value"))
                .ModelShouldMatch(x => x.DiscriminatorValue.ShouldEqual("value"));
        }

        [Test]
        public void DiscriminatorValueShouldDefaultToEntityName()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => {})
                .ModelShouldMatch(x => x.DiscriminatorValue.ShouldEqual("ChildRecord"));
        }

        [Test]
        public void DynamicInsertShouldSetModelDynamicInsertPropertyToTrue()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicInsertShouldSetModelDynamicInsertPropertyToFalse()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Not.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeFalse());
        }

        [Test]
        public void DynamicUpdateShouldSetModelDynamicUpdatePropertyToTrue()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicUpdateShouldSetModelDynamicUpdatePropertyToFalse()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Not.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeFalse());
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(false));
        }

        [Test]
        public void ProxyGenericShouldSetModelProxyPropertyToType()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Proxy<FakeProxyType>())
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void ProxyShouldSetModelProxyPropertyToType()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Proxy(typeof(FakeProxyType)))
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void SelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToTrue()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotSelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToFalse()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Not.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeFalse());
        }

        [Test]
        public void WithTableNameShouldntBreakEvenThoughItIsntSupported()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Table("table"))
                .ModelShouldMatch(x => { });
        }

        [Test]
        public void SchemaIsShouldntBreakEvenThoughItIsntSupported()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Schema("schema"))
                .ModelShouldMatch(x => { });
        }

        [Test]
        public void CheckConstraintShouldntBreakEvenThoughItIsntSupported()
        {
            SubclassMapForSubclass<ChildRecord>()
                .Mapping(m => m.Check("constraint"))
                .ModelShouldMatch(x => { });
        }

        private class FakeProxyType
        {}
    }
}