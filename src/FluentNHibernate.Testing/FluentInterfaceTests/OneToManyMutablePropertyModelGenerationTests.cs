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
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Access.Field());

            mapping.Collections.Single()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void BatchSizeShouldSetModelBatchSizePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .BatchSize(10));

            mapping.Collections.Single()
                .BatchSize.ShouldEqual(10);
        }

        [Test]
        public void CacheShouldSetModelCachePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Cache.ReadOnly());

            mapping.Collections.Single()
                .Cache.Usage.ShouldEqual("read-only");
        }

        [Test]
        public void CascadeShouldSetModelCascadePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Cascade.All());

            mapping.Collections.Single()
                .Cascade.ShouldEqual("all");
        }

        [Test]
        public void CollectionTypeShouldSetModelCollectionTypePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .CollectionType("type"));

            mapping.Collections.Single()
                .CollectionType.ShouldEqual(new TypeReference("type"));
        }

        [Test]
        public void ForeignKeyCascadeOnDeleteShouldSetModelKeyOnDeletePropertyToCascade()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .ForeignKeyCascadeOnDelete());

            mapping.Collections.Single()
                .Key.OnDelete.ShouldEqual("cascade");
        }

        [Test]
        public void InverseShouldSetModelInversePropertyToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Inverse());

            mapping.Collections.Single()
                .Inverse.ShouldBeTrue();
        }

        [Test]
        public void NotInverseShouldSetModelInversePropertyToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Not.Inverse());

            mapping.Collections.Single()
                .Inverse.ShouldBeFalse();
        }

        [Test]
        public void KeyColumnNameShouldAddColumnToModelKeyColumnsCollection()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .KeyColumn("col"));

            mapping.Collections.Single()
                .Key.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void KeyColumnNamesShouldAddColumnsToModelKeyColumnsCollection()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Key(ke => ke.Columns.Add("col1", "col2")));
                
            mapping.Collections.Single()
                .Key.Columns.Count().ShouldEqual(2);
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .LazyLoad());

            mapping.Collections.Single()
                .Lazy.ShouldEqual(Lazy.True);
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Not.LazyLoad());

            mapping.Collections.Single()
                .Lazy.ShouldEqual(Lazy.False);
        }
        
        [Test]
        public void ExtraLazyLoadShouldSetModelLazyPropertyToExtra()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .ExtraLazyLoad());
                
            mapping.Collections.Single()
                .Lazy.ShouldEqual(Lazy.Extra);
        }

        [Test]
        public void NotExtraLazyLoadShouldSetModelLazyPropertyToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Not.ExtraLazyLoad());
                
            mapping.Collections.Single()
                .Lazy.ShouldEqual(Lazy.True);
        }

        [Test]
        public void NotFoundShouldSetModelRelationshipNotFoundPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .NotFound.Ignore());

            mapping.Collections.Single()
                .Relationship.As<OneToManyMapping>()
                .NotFound.ShouldEqual("ignore");
        }

        [Test]
        public void WhereShouldSetModelWherePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Where("x = 1"));
                
            mapping.Collections.Single()
                .Where.ShouldEqual("x = 1");
        }

        [Test]
        public void WithForeignKeyConstraintNameShouldSetModelKeyForeignKeyPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .ForeignKeyConstraintName("fk"));
                
            mapping.Collections.Single()
                .Key.ForeignKey.ShouldEqual("fk");
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Table("t"));
                
            mapping.Collections.Single()
                .TableName.ShouldEqual("t");
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Schema("dbo"));
                
            mapping.Collections.Single()
                .Schema.ShouldEqual("dbo");
        }

        [Test]
        public void FetchShouldSetModelFetchPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Fetch.Select());
                
            mapping.Collections.Single()
                .Fetch.ShouldEqual("select");
        }

        [Test]
        public void PersisterShouldSetModelPersisterPropertyToAssemblyQualifiedName()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Persister<CustomPersister>());
                
            mapping.Collections.Single()
                .Persister.ShouldEqual(new TypeReference(typeof(CustomPersister)));
        }

        [Test]
        public void CheckShouldSetModelCheckToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Check("x > 100"));
                
            mapping.Collections.Single()
                .Check.ShouldEqual("x > 100");
        }

        [Test]
        public void OptimisticLockShouldSetModelOptimisticLockToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .OptimisticLock.All());

            mapping.Collections.Single()
                .OptimisticLock.ShouldEqual("all");
        }

        [Test]
        public void GenericShouldSetModelGenericToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Generic());
                
            mapping.Collections.Single()
                .Generic.ShouldBeTrue();
        }

        [Test]
        public void NotGenericShouldSetModelGenericToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Not.Generic());
                
            mapping.Collections.Single()
                .Generic.ShouldBeFalse();
        }

        [Test]
        public void ReadOnlyShouldSetModelMutableToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .ReadOnly());
                
            mapping.Collections.Single()
                .Mutable.ShouldBeFalse();
        }

        [Test]
        public void NotReadOnlyShouldSetModelMutableToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Not.ReadOnly());
                
            mapping.Collections.Single()
                .Mutable.ShouldBeTrue();
        }

        [Test]
        public void SubselectShouldSetModelSubselectToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Subselect("whee"));
                
            mapping.Collections.Single()
                .Subselect.ShouldEqual("whee");
        }

        [Test]
        public void KeyUpdateShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Key(ke => ke.Update()));
                
            mapping.Collections.Single()
                .Key.Update.ShouldBeTrue();
        }

        [Test]
        public void KeyNullableShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Key(ke => ke.Nullable()));

            mapping.Collections.Single()
                .Key.NotNull.ShouldBeFalse();
        }

        [Test]
        public void KeyNotNullableShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Key(ke => ke.Not.Nullable()));
                
            mapping.Collections.Single()
                .Key.NotNull.ShouldBeTrue();
        }

        [Test]
        public void KeyUniqueShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Key(ke => ke.Unique()));
                
            mapping.Collections.Single()
                .Key.Unique.ShouldBeTrue();
        }

        [Test]
        public void KeyNotUniqueShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Key(ke => ke.Not.Unique()));
                
            mapping.Collections.Single()
                .Key.Unique.ShouldBeFalse();
        }

        [Test]
        public void PropertyRefShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .PropertyRef("prop1"));
                
            mapping.Collections.Single()
                .Key.PropertyRef.ShouldEqual("prop1");
        }

        [Test]
        public void EntityNameShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .EntityName("name"));
                
            mapping.Collections.Single()
                .Relationship.EntityName.ShouldEqual("name");
        }
    }
}