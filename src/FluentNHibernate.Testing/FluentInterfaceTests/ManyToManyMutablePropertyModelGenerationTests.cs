using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToManyMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ShouldSetName()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren));
            var collection = mapping.Collections.Single();
            
            collection.Name.ShouldEqual("BagOfChildren");
        }

        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Access.Field());
            var collection = mapping.Collections.Single();

            collection.Access.ShouldEqual("field");
        }

        [Test]
        public void BatchSizeShouldSetModelBatchSizePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .BatchSize(10));
            var collection = mapping.Collections.Single();

            collection.BatchSize.ShouldEqual(10);
        }

        [Test]
        public void CacheShouldSetModelCachePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Cache.ReadOnly());
            var collection = mapping.Collections.Single();
                    
            collection.Cache.ShouldNotBeNull();
            collection.Cache.Usage.ShouldEqual("read-only");
        }

        [Test]
        public void CascadeShouldSetModelCascadePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Cascade.All());
            var collection = mapping.Collections.Single();
            
            collection.Cascade.ShouldEqual("all");
        }

        [Test]
        public void CollectionTypeShouldSetModelCollectionTypePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .CollectionType("type"));
            var collection = mapping.Collections.Single();
            
            collection.CollectionType.ShouldEqual(new TypeReference("type"));
        }

        [Test]
        public void ForeignKeyCascadeOnDeleteShouldSetModelKeyOnDeletePropertyToCascade()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ForeignKeyCascadeOnDelete());
            var collection = mapping.Collections.Single();
            
            collection.Key.OnDelete.ShouldEqual("cascade");
        }

        [Test]
        public void InverseShouldSetModelInversePropertyToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Inverse());
            var collection = mapping.Collections.Single();
            
            collection.Inverse.ShouldBeTrue();
        }

        [Test]
        public void NotInverseShouldSetModelInversePropertyToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Not.Inverse());
            var collection = mapping.Collections.Single();
            
            collection.Inverse.ShouldBeFalse();
        }

        [Test]
        public void WithParentKeyColumnShouldAddColumnToModelKeyColumnsCollection()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ParentKeyColumn("col"));
            var collection = mapping.Collections.Single();
            
            collection.Key.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void WithForeignKeyConstraintNamesShouldAddForeignKeyToBothColumns()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ForeignKeyConstraintNames("p_fk", "c_fk"));
            var collection = mapping.Collections.Single();
                
            collection.Key.ForeignKey.ShouldEqual("p_fk");
            ((ManyToManyMapping)collection.Relationship).ForeignKey.ShouldEqual("c_fk");
        }

        [Test]
        public void WithChildKeyColumnShouldAddColumnToModelRelationshipColumnsCollection()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ChildKeyColumn("col"));
            var collection = mapping.Collections.Single();
                
            ((ManyToManyMapping)collection.Relationship).Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void LazyLoadShouldSetModelLazyPropertyToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .LazyLoad());
            var collection = mapping.Collections.Single();
            collection.Lazy.ShouldEqual(Lazy.True);
        }

        [Test]
        public void NotLazyLoadShouldSetModelLazyPropertyToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Not.LazyLoad());
            var collection = mapping.Collections.Single();
            collection.Lazy.ShouldEqual(Lazy.False);
        }
        
        [Test]
        public void ExtraLazyLoadShouldSetModelLazyPropertyToExtra()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ExtraLazyLoad());
            var collection = mapping.Collections.Single();
            collection.Lazy.ShouldEqual(Lazy.Extra);
        }

        [Test]
        public void NotExtraLazyLoadShouldSetModelLazyPropertyToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Not.ExtraLazyLoad());
            var collection = mapping.Collections.Single();
            collection.Lazy.ShouldEqual(Lazy.True);
        }

        [Test]
        public void NotFoundShouldSetModelRelationshipNotFoundPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .NotFound.Ignore());
            var collection = mapping.Collections.Single();
                
            ((ManyToManyMapping)collection.Relationship).NotFound.ShouldEqual("ignore");
        }

        [Test]
        public void WhereShouldSetModelWherePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Where("x = 1"));
            var collection = mapping.Collections.Single();
            collection.Where.ShouldEqual("x = 1");
        }

        [Test]
        public void WithTableNameShouldSetModelTableNamePropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Table("t"));
            var collection = mapping.Collections.Single();
            collection.TableName.ShouldEqual("t");
        }

        [Test]
        public void SchemaIsShouldSetModelSchemaPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Schema("dto"));
            var collection = mapping.Collections.Single();
            collection.Schema.ShouldEqual("dto");
        }

        [Test]
        public void FetchShouldSetModelFetchPropertyToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Fetch.Select());
            var collection = mapping.Collections.Single();
            collection.Fetch.ShouldEqual("select");
        }

        [Test]
        public void PersisterShouldSetModelPersisterPropertyToAssemblyQualifiedName()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Persister<CustomPersister>());
            var collection = mapping.Collections.Single();
            collection.Persister.GetUnderlyingSystemType().ShouldEqual(typeof(CustomPersister));
        }

        [Test]
        public void CheckShouldSetModelCheckToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Check("x > 100"));
            var collection = mapping.Collections.Single();
            collection.Check.ShouldEqual("x > 100");
        }

        [Test]
        public void OptimisticLockShouldSetModelOptimisticLockToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .OptimisticLock.All());
            var collection = mapping.Collections.Single();
            collection.OptimisticLock.ShouldEqual("all");
        }

        [Test]
        public void GenericShouldSetModelGenericToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Generic());
            var collection = mapping.Collections.Single();
            collection.Generic.ShouldBeTrue();
        }

        [Test]
        public void NotGenericShouldSetModelGenericToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Not.Generic());
            var collection = mapping.Collections.Single();
            collection.Generic.ShouldBeFalse();
        }

        [Test]
        public void ReadOnlyShouldSetModelMutableToFalse()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ReadOnly());
            var collection = mapping.Collections.Single();
            collection.Mutable.ShouldBeFalse();
        }

        [Test]
        public void NotReadOnlyShouldSetModelMutableToTrue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Not.ReadOnly());
            var collection = mapping.Collections.Single();
            collection.Mutable.ShouldBeTrue();
        }

        [Test]
        public void SubselectShouldSetModelSubselectToValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .Subselect("whee"));
            var collection = mapping.Collections.Single();
            collection.Subselect.ShouldEqual("whee");
        }

        [Test]
        public void OrderByShouldSetAttributeOnBag()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .OrderBy("col1"));
            var collection = mapping.Collections.Single();
           collection.OrderBy.ShouldEqual("col1");
        }

        [Test]
        public void ChildOrderByShouldSetAttributeOnRelationshipModel()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ChildOrderBy("col1"));
            var collection = mapping.Collections.Single();
                
            ((ManyToManyMapping)collection.Relationship).OrderBy.ShouldEqual("col1");
        }

        [Test]
        public void ChildWhereShouldSetAttributeOnRelationshipModel()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .ChildWhere("some condition"));
            var collection = mapping.Collections.Single();

            ((ManyToManyMapping)collection.Relationship).Where.ShouldEqual("some condition");
        }

        [Test]
        public void EntityNameShouldSetModelValue()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.BagOfChildren)
                .EntityName("name"));
            var collection = mapping.Collections.Single();
            collection.Relationship.EntityName.ShouldEqual("name");
        }
    }
}