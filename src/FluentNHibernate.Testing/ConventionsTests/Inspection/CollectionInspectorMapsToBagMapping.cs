using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class CollectionInspectorMapsToBagMapping
    {
        private BagMapping mapping;
        private ICollectionInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new BagMapping();
            inspector = new CollectionInspector(mapping);
        }

        [Test]
        public void AccessMapped()
        {
            mapping.Access = "field";
            inspector.Access.ShouldEqual(Access.Field);
        }

        [Test]
        public void AccessIsSet()
        {
            mapping.Access = "field";
            inspector.IsSet(Prop(x => x.Access))
                .ShouldBeTrue();
        }

        [Test]
        public void AccessIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Access))
                .ShouldBeFalse();
        }

        [Test]
        public void BatchSizeMapped()
        {
            mapping.BatchSize = 10;
            inspector.BatchSize.ShouldEqual(10);
        }

        [Test]
        public void BatchSizeIsSet()
        {
            mapping.BatchSize = 10;
            inspector.IsSet(Prop(x => x.BatchSize))
                .ShouldBeTrue();
        }

        [Test]
        public void BatchSizeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.BatchSize))
                .ShouldBeFalse();
        }

        [Test]
        public void CacheMapped()
        {
            mapping.Cache = new CacheMapping();
            mapping.Cache.Usage = "value";
            inspector.Cache.Usage.ShouldEqual("value");
        }

        [Test]
        public void CacheIsSet()
        {
            mapping.Cache = new CacheMapping();
            mapping.Cache.Usage = "value";
            inspector.IsSet(Prop(x => x.Cache))
                .ShouldBeTrue();
        }

        [Test]
        public void CacheIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Cache))
                .ShouldBeFalse();
        }

        [Test]
        public void CascadeMapped()
        {
            mapping.Cascade = "all";
            inspector.Cascade.ShouldEqual(Cascade.All);
        }

        [Test]
        public void CascadeIsSet()
        {
            mapping.Cascade = "all";
            inspector.IsSet(Prop(x => x.Cascade))
                .ShouldBeTrue();
        }

        [Test]
        public void CascadeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Cascade))
                .ShouldBeFalse();
        }

        [Test]
        public void CheckMapped()
        {
            mapping.Check = "value";
            inspector.Check.ShouldEqual("value");
        }

        [Test]
        public void CheckIsSet()
        {
            mapping.Check = "value";
            inspector.IsSet(Prop(x => x.Check))
                .ShouldBeTrue();
        }

        [Test]
        public void CheckIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Check))
                .ShouldBeFalse();
        }

        [Test]
        public void ChildTypeMapped()
        {
            mapping.ChildType = typeof(ExampleClass);
            inspector.ChildType.ShouldEqual(typeof(ExampleClass));
        }

        [Test]
        public void ChildTypeIsSet()
        {
            mapping.ChildType = typeof(ExampleClass);
            inspector.IsSet(Prop(x => x.ChildType))
                .ShouldBeTrue();
        }

        [Test]
        public void ChildTypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.ChildType))
                .ShouldBeFalse();
        }

        [Test]
        public void CollectionTypeMapped()
        {
            mapping.CollectionType = new TypeReference(typeof(IList<ExampleClass>));
            inspector.CollectionType.ShouldEqual(new TypeReference(typeof(IList<ExampleClass>)));
        }

        [Test]
        public void CollectionTypeIsSet()
        {
            mapping.CollectionType = new TypeReference(typeof(IList<ExampleClass>));
            inspector.IsSet(Prop(x => x.CollectionType))
                .ShouldBeTrue();
        }

        [Test]
        public void CollectionTypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.CollectionType))
                .ShouldBeFalse();
        }

        [Test]
        public void CompositeElementMapped()
        {
            mapping.CompositeElement = new CompositeElementMapping();
            mapping.CompositeElement.Class = new TypeReference(typeof(ExampleClass));
            inspector.CompositeElement.Class.ShouldEqual(new TypeReference(typeof(ExampleClass)));
        }

        [Test]
        public void CompositeElementIsSet()
        {
            mapping.CompositeElement = new CompositeElementMapping();
            mapping.CompositeElement.Class = new TypeReference(typeof(ExampleClass));
            inspector.IsSet(Prop(x => x.CompositeElement))
                .ShouldBeTrue();
        }

        [Test]
        public void CompositeElementIsNotSet()
        {
            inspector.IsSet(Prop(x => x.CompositeElement))
                .ShouldBeFalse();
        }

        [Test]
        public void ElementMapped()
        {
            mapping.Element = new ElementMapping();
            mapping.Element.Type = new TypeReference(typeof(ExampleClass));
            inspector.Element.Type.ShouldEqual(new TypeReference(typeof(ExampleClass)));
        }

        [Test]
        public void ElementIsSet()
        {
            mapping.Element = new ElementMapping();
            mapping.Element.Type = new TypeReference(typeof(ExampleClass));
            inspector.IsSet(Prop(x => x.Element))
                .ShouldBeTrue();
        }

        [Test]
        public void ElementIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Element))
                .ShouldBeFalse();
        }

        [Test]
        public void FetchMapped()
        {
            mapping.Fetch = "join";
            inspector.Fetch.ShouldEqual(Fetch.Join);
        }

        [Test]
        public void FetchIsSet()
        {
            mapping.Fetch = "join";
            inspector.IsSet(Prop(x => x.Fetch))
                .ShouldBeTrue();
        }

        [Test]
        public void FetchIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Fetch))
                .ShouldBeFalse();
        }

        [Test]
        public void GenericMapped()
        {
            mapping.Generic = true;
            inspector.Generic.ShouldEqual(true);
        }

        [Test]
        public void GenericIsSet()
        {
            mapping.Generic = true;
            inspector.IsSet(Prop(x => x.Generic))
                .ShouldBeTrue();
        }

        [Test]
        public void GenericIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Generic))
                .ShouldBeFalse();
        }

        [Test]
        public void InverseMapped()
        {
            mapping.Inverse = true;
            inspector.Inverse.ShouldEqual(true);
        }

        [Test]
        public void InverseIsSet()
        {
            mapping.Inverse = true;
            inspector.IsSet(Prop(x => x.Inverse))
                .ShouldBeTrue();
        }

        [Test]
        public void InverseIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Inverse))
                .ShouldBeFalse();
        }

        [Test]
        public void KeyMapped()
        {
            mapping.Key = new KeyMapping();
            mapping.Key.ForeignKey = "key";
            inspector.Key.ForeignKey.ShouldEqual("key");
        }

        [Test]
        public void KeyIsSet()
        {
            mapping.Key = new KeyMapping();
            mapping.Key.ForeignKey = "key";
            inspector.IsSet(Prop(x => x.Key))
                .ShouldBeTrue();
        }

        [Test]
        public void KeyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Key))
                .ShouldBeFalse();
        }

        [Test]
        public void LazyMapped()
        {
            mapping.Lazy = true;
            inspector.LazyLoad.ShouldEqual(true);
        }

        [Test]
        public void LazyIsSet()
        {
            mapping.Lazy = true;
            inspector.IsSet(Prop(x => x.LazyLoad))
                .ShouldBeTrue();
        }

        [Test]
        public void LazyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.LazyLoad))
                .ShouldBeFalse();
        }

        [Test]
        public void NameMapped()
        {
            mapping.Name = "name";
            inspector.Name.ShouldEqual("name");
        }

        [Test]
        public void NameIsSet()
        {
            mapping.Name = "name";
            inspector.IsSet(Prop(x => x.Name))
                .ShouldBeTrue();
        }

        [Test]
        public void NameIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Name))
                .ShouldBeFalse();
        }

        [Test]
        public void OptimisticLockMapped()
        {
            mapping.OptimisticLock = "all";
            inspector.OptimisticLock.ShouldEqual(OptimisticLock.All);
        }

        [Test]
        public void OptimisticLockIsSet()
        {
            mapping.OptimisticLock = "all";
            inspector.IsSet(Prop(x => x.OptimisticLock))
                .ShouldBeTrue();
        }

        [Test]
        public void OptimisticLockIsNotSet()
        {
            inspector.IsSet(Prop(x => x.OptimisticLock))
                .ShouldBeFalse();
        }

        [Test]
        public void PersisterMapped()
        {
            mapping.Persister = new TypeReference("persister");
            inspector.Persister.ShouldEqual(new TypeReference("persister"));
        }

        [Test]
        public void PersisterIsSet()
        {
            mapping.Persister = new TypeReference("persister");
            inspector.IsSet(Prop(x => x.Persister))
                .ShouldBeTrue();
        }

        [Test]
        public void PersisterIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Persister))
                .ShouldBeFalse();
        }

        [Test]
        public void RelationshipMapped()
        {
            mapping.Relationship = new ManyToManyMapping();
            mapping.Relationship.Class = new TypeReference(typeof(ExampleClass));
            inspector.Relationship.Class.ShouldEqual(new TypeReference(typeof(ExampleClass)));
        }

        [Test]
        public void RelationshipIsSet()
        {
            mapping.Relationship = new ManyToManyMapping();
            mapping.Relationship.Class = new TypeReference(typeof(ExampleClass));
            inspector.IsSet(Prop(x => x.Relationship))
                .ShouldBeTrue();
        }

        [Test]
        public void RelationshipIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Relationship))
                .ShouldBeFalse();
        }

        [Test]
        public void SchemaMapped()
        {
            mapping.Schema = "dbo";
            inspector.Schema.ShouldEqual("dbo");
        }

        [Test]
        public void SchemaIsSet()
        {
            mapping.Schema = "dbo";
            inspector.IsSet(Prop(x => x.Schema))
                .ShouldBeTrue();
        }

        [Test]
        public void SchemaIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Schema))
                .ShouldBeFalse();
        }

        [Test]
        public void TableNameMapped()
        {
            mapping.TableName = "table";
            inspector.TableName.ShouldEqual("table");
        }

        [Test]
        public void TableNameIsSet()
        {
            mapping.TableName = "table";
            inspector.IsSet(Prop(x => x.TableName))
                .ShouldBeTrue();
        }

        [Test]
        public void TableNameIsNotSet()
        {
            inspector.IsSet(Prop(x => x.TableName))
                .ShouldBeFalse();
        }

        [Test]
        public void WhereMapped()
        {
            mapping.Where = "x = 1";
            inspector.Where.ShouldEqual("x = 1");
        }

        [Test]
        public void WhereIsSet()
        {
            mapping.Where = "x = 1";
            inspector.IsSet(Prop(x => x.Where))
                .ShouldBeTrue();
        }

        [Test]
        public void WhereIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Where))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<ICollectionInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}