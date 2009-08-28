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
}