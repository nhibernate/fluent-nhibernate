using System;
using System.Linq.Expressions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class CollectionInspectorMapsToCollectionMapping
    {
        private CollectionMapping mapping;
        private ICollectionInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = CollectionMapping.Bag();
            inspector = new CollectionInspector(mapping);
        }
        [Test]
        public void MapsIndexToInspector()
        {
            mapping.Index = new IndexMapping();
            inspector.Index.ShouldBeOfType<IIndexInspector>();
        }

        [Test]
        public void IndexIsSet()
        {
            mapping.Index = new IndexMapping();
            inspector.IsSet(Prop(x => x.Index))
                .ShouldBeTrue();
        }

        [Test]
        public void IndexIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Index))
                .ShouldBeFalse();
        }

        [Test]
        public void MapsIndexManyToManyToInspector()
        {
            mapping.Index = new IndexManyToManyMapping();
            inspector.Index.ShouldBeOfType<IIndexManyToManyInspector>();
        }

        [Test]
        public void IndexManyToManyIsSet()
        {
            mapping.Index = new IndexManyToManyMapping();
            inspector.IsSet(Prop(x => x.Index))
                .ShouldBeTrue();
        }

        [Test]
        public void OrderByIsSet()
        {
            mapping.OrderBy = "AField";
            inspector.IsSet(Prop(x => x.OrderBy))
                .ShouldBeTrue();
        }

        [Test]
        public void OrderByIsNotSet()
        {
            inspector.IsSet(Prop(x => x.OrderBy))
                .ShouldBeFalse();
        }

        [Test]
        public void SortByIsSet()
        {
            mapping.Sort = "AField";
            inspector.IsSet(Prop(x => x.Sort))
                .ShouldBeTrue();
        }

        [Test]
        public void SortByIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Sort))
                .ShouldBeFalse();
        }

        #region Helpers

        static Member Prop(Expression<Func<ICollectionInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetMember(propertyExpression);
        }

        #endregion
    }
}