using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManyMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Access.Field())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void BatchSizeShouldSetModelBatchSizePropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.BatchSize(10))
                .ModelShouldMatch(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CacheShouldSetModelCachePropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Cache.ReadOnly())
                .ModelShouldMatch(x =>
                {
                    x.Cache.ShouldNotBeNull();
                    x.Cache.Usage.ShouldEqual("read-only");
                });
        }

        [Test]
        public void CascadeShouldSetModelCascadePropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Cascade.All())
                .ModelShouldMatch(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void CollectionTypeShouldSetModelCollectionTypePropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.CollectionType("type"))
                .ModelShouldMatch(x => x.CollectionType.ShouldEqual(new TypeReference("type")));
        }

        [Test]
        public void ForeignKeyCascadeOnDeleteShouldSetModelKeyOnDeletePropertyToCascade()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.ForeignKeyCascadeOnDelete())
                .ModelShouldMatch(x => x.Key.OnDelete.ShouldEqual("cascade"));
        }

        [Test]
        public void InverseShouldSetModelInversePropertyToTrue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void NotInverseShouldSetModelInversePropertyToFalse()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Not.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeFalse());
        }

        [Test]
        public void KeyColumnNameShouldAddColumnToModelKeyColumnsCollection()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.KeyColumn("col"))
                .ModelShouldMatch(x => x.Key.Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void KeyColumnNamesShouldAddColumnsToModelKeyColumnsCollection()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.KeyColumns.Add("col1", "col2"))
                .ModelShouldMatch(x => x.Key.Columns.Count().ShouldEqual(2));
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(false));
        }

        [Test]
        public void NotFoundShouldSetModelRelationshipNotFoundPropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.NotFound.Ignore())
                .ModelShouldMatch(x => ((OneToManyMapping)x.Relationship).NotFound.ShouldEqual("ignore"));
        }

        [Test]
        public void WhereShouldSetModelWherePropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Where("x = 1"))
                .ModelShouldMatch(x => x.Where.ShouldEqual("x = 1"));
        }

        [Test]
        public void WithForeignKeyConstraintNameShouldSetModelKeyForeignKeyPropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.ForeignKeyConstraintName("fk"))
                .ModelShouldMatch(x => x.Key.ForeignKey.ShouldEqual("fk"));
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Table("t"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("t"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Schema("dto"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("dto"));
        }

        [Test]
        public void FetchShouldSetModelFetchPropertyToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Fetch.Select())
                .ModelShouldMatch(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void PersisterShouldSetModelPersisterPropertyToAssemblyQualifiedName()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Persister<CustomPersister>())
                .ModelShouldMatch(x => x.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(CustomPersister)));
        }

        [Test]
        public void CheckShouldSetModelCheckToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Check("x > 100"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("x > 100"));
        }

        [Test]
        public void OptimisticLockShouldSetModelOptimisticLockToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.OptimisticLock.All())
                .ModelShouldMatch(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void GenericShouldSetModelGenericToTrue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Generic())
                .ModelShouldMatch(x => x.Generic.ShouldBeTrue());
        }

        [Test]
        public void NotGenericShouldSetModelGenericToFalse()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Not.Generic())
                .ModelShouldMatch(x => x.Generic.ShouldBeFalse());
        }

        [Test]
        public void ReadOnlyShouldSetModelMutableToFalse()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.ReadOnly())
                .ModelShouldMatch(x => x.Mutable.ShouldBeFalse());
        }

        [Test]
        public void NotReadOnlyShouldSetModelMutableToTrue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Not.ReadOnly())
                .ModelShouldMatch(x => x.Mutable.ShouldBeTrue());
        }

        [Test]
        public void SubselectShouldSetModelSubselectToValue()
        {
            OneToMany<ChildObject>(x => x.BagOfChildren)
                .Mapping(m => m.Subselect("whee"))
                .ModelShouldMatch(x => x.Subselect.ShouldEqual("whee"));
        }
    }
}