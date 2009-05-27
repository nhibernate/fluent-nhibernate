using System.Linq;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToManyMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Access.AsField())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void BatchSizeShouldSetModelBatchSizePropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.BatchSize(10))
                .ModelShouldMatch(x => x.BatchSize.ShouldEqual(10));
        }

        [Test]
        public void CacheShouldSetModelCachePropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
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
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Cascade.All())
                .ModelShouldMatch(x => x.Cascade.ShouldEqual("all"));
        }

        [Test]
        public void CollectionTypeShouldSetModelCollectionTypePropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.CollectionType("type"))
                .ModelShouldMatch(x => x.CollectionType.ShouldEqual("type"));
        }

        [Test]
        public void ForeignKeyCascadeOnDeleteShouldSetModelKeyOnDeletePropertyToCascade()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.ForeignKeyCascadeOnDelete())
                .ModelShouldMatch(x => x.Key.OnDelete.ShouldEqual("cascade"));
        }

        [Test]
        public void InverseShouldSetModelInversePropertyToTrue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void NotInverseShouldSetModelInversePropertyToFalse()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Not.Inverse())
                .ModelShouldMatch(x => x.Inverse.ShouldBeFalse());
        }

        [Test]
        public void WithParentKeyColumnShouldAddColumnToModelKeyColumnsCollection()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.WithParentKeyColumn("col"))
                .ModelShouldMatch(x => x.Key.Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void WithForeignKeyConstraintNamesShouldAddForeignKeyToBothColumns()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.WithForeignKeyConstraintNames("p_fk", "c_fk"))
                .ModelShouldMatch(x =>
                {
                    x.Key.ForeignKey.ShouldEqual("p_fk");
                    ((ManyToManyMapping)x.Relationship).ForeignKey.ShouldEqual("c_fk");
                });
        }

        [Test]
        public void WithChildKeyColumnShouldAddColumnToModelRelationshipColumnsCollection()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.WithChildKeyColumn("col"))
                .ModelShouldMatch(x => ((ManyToManyMapping)x.Relationship).Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeTrue());
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Not.LazyLoad())
                .ModelShouldMatch(x => x.Lazy.ShouldBeFalse());
        }

        [Test]
        public void NotFoundShouldSetModelRelationshipNotFoundPropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.NotFound.Ignore())
                .ModelShouldMatch(x => ((ManyToManyMapping)x.Relationship).NotFound.ShouldEqual("ignore"));
        }

        [Test]
        public void WhereShouldSetModelWherePropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Where("x = 1"))
                .ModelShouldMatch(x => x.Where.ShouldEqual("x = 1"));
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.WithTableName("t"))
                .ModelShouldMatch(x => x.TableName.ShouldEqual("t"));
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.SchemaIs("dto"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("dto"));
        }

        [Test]
        public void OuterJoinShouldSetModelOuterJoinPropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.OuterJoin.Auto())
                .ModelShouldMatch(x => x.OuterJoin.ShouldEqual("auto"));
        }

        [Test]
        public void FetchShouldSetModelFetchPropertyToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Fetch.Select())
                .ModelShouldMatch(x => x.Fetch.ShouldEqual("select"));
        }

        [Test]
        public void PersisterShouldSetModelPersisterPropertyToAssemblyQualifiedName()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Persister<CustomPersister>())
                .ModelShouldMatch(x => x.Persister.ShouldEqual(typeof(CustomPersister).AssemblyQualifiedName));
        }

        [Test]
        public void CheckShouldSetModelCheckToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Check("x > 100"))
                .ModelShouldMatch(x => x.Check.ShouldEqual("x > 100"));
        }

        [Test]
        public void OptimisticLockShouldSetModelOptimisticLockToValue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.OptimisticLock.All())
                .ModelShouldMatch(x => x.OptimisticLock.ShouldEqual("all"));
        }

        [Test]
        public void GenericShouldSetModelGenericToTrue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Generic())
                .ModelShouldMatch(x => x.Generic.ShouldBeTrue());
        }

        [Test]
        public void NotGenericShouldSetModelGenericToTrue()
        {
            ManyToMany<ManyToManyTarget>(x => x.BagOfChildren)
                .Mapping(m => m.Not.Generic())
                .ModelShouldMatch(x => x.Generic.ShouldBeFalse());
        }
    }
}