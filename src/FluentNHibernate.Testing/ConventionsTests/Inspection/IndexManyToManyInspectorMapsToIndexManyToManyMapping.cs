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
    public class IndexManyToManyInspectorMapsToIndexManyToManyMapping
    {
        private IndexManyToManyMapping mapping;
        private IIndexManyToManyInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new IndexManyToManyMapping();
            inspector = new IndexManyToManyInspector(mapping);
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
        public void ClassMapped()
        {
            mapping.Class = new TypeReference(typeof(ExampleClass));
            inspector.Class.ShouldEqual(new TypeReference(typeof(ExampleClass)));
        }

        [Test]
        public void ClassIsSet()
        {
            mapping.Class = new TypeReference(typeof(ExampleClass));
            inspector.IsSet(Prop(x => x.Class))
                .ShouldBeTrue();
        }

        [Test]
        public void TypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Class))
                .ShouldBeFalse();
        }

        [Test]
        public void StringIdentifierForModelIsTypeName()
        {
            mapping.Class = new TypeReference(typeof(ExampleClass));
            inspector.StringIdentifierForModel
                .ShouldEqual(mapping.Class.Name);
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

        [Test]
        public void ForeignKeyIsSet()
        {
            mapping.ForeignKey = "FKTest";
            inspector.ForeignKey
                .ShouldEqual("FKTest");
        }

        [Test]
        public void ForeignKeyIsNotSet()
        {
            inspector.ForeignKey
                .ShouldBeNull();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IIndexManyToManyInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}