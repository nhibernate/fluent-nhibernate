using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class HibernateMappingInspectorMapsToHibernateMapping
    {
        private HibernateMapping mapping;
        private IHibernateMappingInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new HibernateMapping();
            inspector = new HibernateMappingInspector(mapping);
        }

        [Test]
        public void CatalogMapped()
        {
            mapping.Catalog = "cat";
            inspector.Catalog.ShouldEqual("cat");
        }

        [Test]
        public void CatalogIsSet()
        {
            mapping.Catalog = "cat";
            inspector.IsSet(Prop(x => x.Catalog))
                .ShouldBeTrue();
        }

        [Test]
        public void CatalogIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Catalog))
                .ShouldBeFalse();
        }

        [Test]
        public void DefaultAccessMapped()
        {
            mapping.DefaultAccess = "field";
            inspector.DefaultAccess.ShouldEqual(Access.Field);
        }

        [Test]
        public void DefaultAccessIsSet()
        {
            mapping.DefaultAccess = "field";
            inspector.IsSet(Prop(x => x.DefaultAccess))
                .ShouldBeTrue();
        }

        [Test]
        public void DefaultAccessIsNotSet()
        {
            inspector.IsSet(Prop(x => x.DefaultAccess))
                .ShouldBeFalse();
        }

        [Test]
        public void DefaultCascadeMapped()
        {
            mapping.DefaultCascade = "all";
            inspector.DefaultCascade.ShouldEqual(Cascade.All);
        }

        [Test]
        public void DefaultCascadeIsSet()
        {
            mapping.DefaultCascade = "all";
            inspector.IsSet(Prop(x => x.DefaultCascade))
                .ShouldBeTrue();
        }

        [Test]
        public void DefaultCascadeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.DefaultCascade))
                .ShouldBeFalse();
        }

        [Test]
        public void DefaultLazyMapped()
        {
            mapping.DefaultLazy = true;
            inspector.DefaultLazy.ShouldEqual(true);
        }

        [Test]
        public void DefaultLazyIsSet()
        {
            mapping.DefaultLazy = true;
            inspector.IsSet(Prop(x => x.DefaultLazy))
                .ShouldBeTrue();
        }

        [Test]
        public void DefaultLazyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.DefaultLazy))
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

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IHibernateMappingInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
    [TestFixture, Category("Inspection DSL")]
    public class VersionInspectorMapsToVersionMapping
    {
        private VersionMapping mapping;
        private IVersionInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new VersionMapping();
            inspector = new VersionInspector(mapping);
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
        public void ColumnMapped()
        {
            mapping.Column = "col";
            inspector.Column.ShouldEqual("col");
        }

        [Test]
        public void ColumnIsSet()
        {
            mapping.Column = "col";
            inspector.IsSet(Prop(x => x.Column))
                .ShouldBeTrue();
        }

        [Test]
        public void ColumnIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Column))
                .ShouldBeFalse();
        }

        [Test]
        public void GeneratedMapped()
        {
            mapping.Generated = "insert";
            inspector.Generated.ShouldEqual(Generated.Insert);
        }

        [Test]
        public void GeneratedIsSet()
        {
            mapping.Generated = "insert";
            inspector.IsSet(Prop(x => x.Generated))
                .ShouldBeTrue();
        }

        [Test]
        public void GeneratedIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Generated))
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
        public void TypeMapped()
        {
            mapping.Type = new TypeReference(typeof(string));
            inspector.Type.ShouldEqual(new TypeReference(typeof(string)));
        }

        [Test]
        public void TypeIsSet()
        {
            mapping.Type = new TypeReference(typeof(string));
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
        public void UnsavedValueMapped()
        {
            mapping.UnsavedValue = "test";
            inspector.UnsavedValue.ShouldEqual("test");
        }

        [Test]
        public void UnsavedValueIsSet()
        {
            mapping.UnsavedValue = "test";
            inspector.IsSet(Prop(x => x.UnsavedValue))
                .ShouldBeTrue();
        }

        [Test]
        public void UnsavedValueIsNotSet()
        {
            inspector.IsSet(Prop(x => x.UnsavedValue))
                .ShouldBeFalse();
        }

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IVersionInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}