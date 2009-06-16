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
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Access.AsField())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void BatchSizeShouldSetModelBatchSizePropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.BatchSize(10))
                .ModelShouldMatch(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CacheShouldSetModelCachePropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Cache.AsReadOnly())
                .ModelShouldMatch(x =>
                {
                    x.Cache.ShouldNotBeNull();
                    x.Cache.Usage.ShouldEqual("read-only");
                });
        }

        [Test]
        public void CascadeShouldSetModelCascadePropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Cascade.All())
                .ModelShouldMatch(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void CollectionTypeShouldSetModelCollectionTypePropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.CollectionType("type"))
                .ModelShouldMatch(x => x.CollectionType.ShouldEqual(new TypeReference("type")));
        }

        [Test]
        public void ForeignKeyCascadeOnDeleteShouldSetModelKeyOnDeletePropertyToCascade()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.ForeignKeyCascadeOnDelete())
                .ModelShouldMatch(x => x.Key.OnDelete.ShouldEqual("cascade"));
        }

        [Test]
        public void InverseShouldSetModelInversePropertyToTrue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void NotInverseShouldSetModelInversePropertyToFalse()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Not.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeFalse());
        }

        [Test]
        public void KeyColumnNameShouldAddColumnToModelKeyColumnsCollection()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.KeyColumnName("col"))
                .ModelShouldMatch(x => x.Key.Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void KeyColumnNamesShouldAddColumnsToModelKeyColumnsCollection()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.KeyColumnNames.Add("col1", "col2"))
                .ModelShouldMatch(x => x.Key.Columns.Count().ShouldEqual(2));
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(Laziness.True));
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldEqual(Laziness.False));
        }

        [Test]
        public void NotFoundShouldSetModelRelationshipNotFoundPropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.NotFound.Ignore())
                .ModelShouldMatch(x => ((OneToManyMapping)x.Relationship).NotFound.ShouldEqual("ignore"));
        }

        [Test]
        public void WhereShouldSetModelWherePropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Where("x = 1"))
                .ModelShouldMatch(x => x.Where.ShouldEqual("x = 1"));
        }

        [Test]
        public void WithForeignKeyConstraintNameShouldSetModelKeyForeignKeyPropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.WithForeignKeyConstraintName("fk"))
                .ModelShouldMatch(x => x.Key.ForeignKey.ShouldEqual("fk"));
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.WithTableName("t"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("t"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.SchemaIs("dto"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("dto"));
        }

        [Test]
        public void OuterJoinShouldSetModelOuterJoinPropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.OuterJoin.Auto())
                .ModelShouldMatch(x => x.OuterJoin.ShouldEqual("auto"));
        }

        [Test]
        public void FetchShouldSetModelFetchPropertyToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Fetch.Select())
                .ModelShouldMatch(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void PersisterShouldSetModelPersisterPropertyToAssemblyQualifiedName()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Persister<CustomPersister>())
                .ModelShouldMatch(x => x.Persister.ShouldEqual(typeof(CustomPersister).AssemblyQualifiedName));
        }

        [Test]
        public void CheckShouldSetModelCheckToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Check("x > 100"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("x > 100"));
        }

        [Test]
        public void OptimisticLockShouldSetModelOptimisticLockToValue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.OptimisticLock.All())
                .ModelShouldMatch(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void GenericShouldSetModelGenericToTrue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Generic())
                .ModelShouldMatch(x => x.Generic.ShouldBeTrue());
        }

        [Test]
        public void NotGenericShouldSetModelGenericToTrue()
        {
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Not.Generic())
                .ModelShouldMatch(x => x.Generic.ShouldBeFalse());
        }
    }
}