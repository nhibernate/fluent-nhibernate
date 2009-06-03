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
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void NotAbstractShouldSetModelAbstractPropertyToFalse()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Not.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeFalse());
        }

        [Test]
        public void DynamicInsertShouldSetModelDynamicInsertPropertyToTrue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicInsertShouldSetModelDynamicInsertPropertyToFalse()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Not.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeFalse());
        }

        [Test]
        public void DynamicUpdateShouldSetModelDynamicUpdatePropertyToTrue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicUpdateShouldSetModelDynamicUpdatePropertyToFalse()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Not.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeFalse());
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeTrue());
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeFalse());
        }

        [Test]
        public void ProxyGenericShouldSetModelProxyPropertyToType()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Proxy<FakeProxyType>())
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void ProxyShouldSetModelProxyPropertyToType()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Proxy(typeof(FakeProxyType)))
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType).AssemblyQualifiedName));
        }

        [Test]
        public void SelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToTrue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotSelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToFalse()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.Not.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeFalse());
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.WithTableName("table"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("table"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.SchemaIs("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }

        [Test]
        public void CheckConstraintShouldSetModelCheckPropertyToValue()
        {
            JoinedSubClass<ChildRecord>()
                .Mapping(m => m.CheckConstraint("constraint"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("constraint"));
        }

        private class FakeProxyType
        {}
    }
}