using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class JoinedSubclassMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AbstractShouldSetModelAbstractPropertyToTrue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void NotAbstractShouldSetModelAbstractPropertyToFalse()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeFalse());
        }

        [Test]
        public void DynamicInsertShouldSetModelDynamicInsertPropertyToTrue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicInsertShouldSetModelDynamicInsertPropertyToFalse()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeFalse());
        }

        [Test]
        public void DynamicUpdateShouldSetModelDynamicUpdatePropertyToTrue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicUpdateShouldSetModelDynamicUpdatePropertyToFalse()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeFalse());
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(false));
        }

        [Test]
        public void ProxyGenericShouldSetModelProxyPropertyToType()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Proxy<FakeProxyType>())
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void ProxyShouldSetModelProxyPropertyToType()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Proxy(typeof(FakeProxyType)))
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void SelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToTrue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotSelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToFalse()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Not.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeFalse());
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Table("table"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("table"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.Schema("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }

        [Test]
        public void CheckConstraintShouldSetModelCheckPropertyToValue()
        {
            JoinedSubclass<ChildRecord>()
                .Mapping(m => m.CheckConstraint("constraint"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("constraint"));
        }

        private class FakeProxyType
        {}
    }
}