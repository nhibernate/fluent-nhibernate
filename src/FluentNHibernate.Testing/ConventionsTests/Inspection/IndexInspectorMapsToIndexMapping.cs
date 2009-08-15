using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class IndexInspectorMapsToIndexMapping
    {
        private IndexMapping mapping;
        private IIndexInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new IndexMapping();
            inspector = new IndexInspector(mapping);
        }

        [Test]
        public void ColumnsCollectionHasSameCountAsMapping()
        {
            mapping.AddColumn(new ColumnMapping());
            inspector.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void ColumnsCollectionOfInspectors()
        {
            mapping.AddColumn(new ColumnMapping());
            inspector.Columns.First().ShouldBeOfType<IColumnInspector>();
        }

        [Test]
        public void ColumnsCollectionIsEmpty()
        {
            inspector.Columns.IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void TypeMapped()
        {
            mapping.Type = new TypeReference(typeof(ExampleClass));
            inspector.Type.ShouldEqual(new TypeReference(typeof(ExampleClass)));
        }

        [Test]
        public void TypeIsSet()
        {
            mapping.Type = new TypeReference(typeof(ExampleClass));
            inspector.IsSet(Prop(x => x.Type))
                .ShouldBeTrue();
        }

        [Test]
        public void TypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Type))
                .ShouldBeFalse();
        }

        [Test]
        public void StringIdentifierForModelIsTypeName()
        {
            mapping.Type = new TypeReference(typeof(ExampleClass));
            inspector.StringIdentifierForModel
                .ShouldEqual(mapping.Type.Name);
        }

        [Test]
        public void ContainingEntityTypeIsSet()
        {
            mapping.ContainingEntityType = typeof(OneToManyMapping);
            inspector.EntityType
                .ShouldEqual(typeof(OneToManyMapping));
        }

        [Test]
        public void ContainingEntityTypeIsNotSet()
        {
            inspector.EntityType
                .ShouldBeNull();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IIndexInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}