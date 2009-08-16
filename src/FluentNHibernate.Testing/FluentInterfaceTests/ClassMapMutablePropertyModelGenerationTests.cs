using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ClassMapMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void BatchSizeSetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.BatchSize(10))
                .ModelShouldMatch(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CheckSetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.CheckConstraint("constraint"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("constraint"));
        }

        [Test]
        public void OptimisticLockSetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.OptimisticLock.All())
                .ModelShouldMatch(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void PersisterSetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Persister<CustomPersister>())
                .ModelShouldMatch(x => x.Persister.ShouldEqual(typeof(CustomPersister).AssemblyQualifiedName));
        }

        [Test]
        public void PolymorphismSetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Polymorphism.Implicit())
                .ModelShouldMatch(x => x.Polymorphism.ShouldEqual("implicit"));
        }

        [Test]
        public void ProxySetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Proxy<FakeProxy>())
                .ModelShouldMatch(x => x.Proxy.ShouldEqual(typeof(FakeProxy).AssemblyQualifiedName));
        }

        [Test]
        public void SelectBeforeUpdateSetsPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.SelectBeforeUpdate())
                .ModelShouldMatch(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void LazyLoadSetsModelPropertyToTrue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotLazyLoadSetsModelPropertyToFalse()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(false));
        }

        [Test]
        public void WithTableShouldSetModelPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Table("table"))
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
                .Mapping(x => x.Schema("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }

        [Test]
        public void SubselectShouldSetModelPropertyToValue()
        {
            ClassMap<PropertyTarget>()
                .Mapping(x => x.Subselect("sql query"))
                .ModelShouldMatch(x => x.Subselect.ShouldEqual("sql query"));
        }

        public class FakeProxy
        { }
    }
}
