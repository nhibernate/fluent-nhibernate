using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class SubclassMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AbstractShouldSetModelAbstractPropertyToTrue()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void NotAbstractShouldSetModelAbstractPropertyToFalse()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Not.Abstract())
                .ModelShouldMatch(x => x.Abstract.ShouldBeFalse());
        }

        [Test]
        public void DynamicInsertShouldSetModelDynamicInsertPropertyToTrue()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicInsertShouldSetModelDynamicInsertPropertyToFalse()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Not.DynamicInsert())
                .ModelShouldMatch(x => x.DynamicInsert.ShouldBeFalse());
        }

        [Test]
        public void DynamicUpdateShouldSetModelDynamicUpdatePropertyToTrue()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotDynamicUpdateShouldSetModelDynamicUpdatePropertyToFalse()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Not.DynamicUpdate())
                .ModelShouldMatch(x => x.DynamicUpdate.ShouldBeFalse());
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeTrue());
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeFalse());
        }

        [Test]
        public void ProxyGenericShouldSetModelProxyPropertyToType()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Proxy<FakeProxyType>())
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType)));
        }

        [Test]
        public void ProxyShouldSetModelProxyPropertyToType()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Proxy(typeof(FakeProxyType)))
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxyType)));
        }

        [Test]
        public void SelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToTrue()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void NotSelectBeforeUpdateShouldSetModelSelectBeforeUpdatePropertyToFalse()
        {
            SubClass<ChildRecord>()
                .Mapping(m => m.Not.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeFalse());
        }

        private class FakeProxyType
        {}
    }
}