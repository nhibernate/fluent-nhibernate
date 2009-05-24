using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class PropertyDslMapsToPropertyMapping
    {
        private PropertyMapping mapping;
        private IPropertyInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new PropertyMapping(typeof(Record));
            inspector = new PropertyDsl(mapping);
        }

        [Test]
        public void AccessMapped()
        {
            mapping.Access = "field";
            inspector.Access.ShouldEqual(Access.FromString(mapping.Access));
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
        public void CustomTypeMappedToType()
        {
            mapping.Type = "type";
            inspector.CustomType.ShouldEqual(mapping.Type);
        }

        [Test]
        public void CustomTypeIsSet()
        {
            mapping.Type = "type";
            inspector.IsSet(Prop(x => x.CustomType))
                .ShouldBeTrue();
        }

        [Test]
        public void CustomTypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.CustomType))
                .ShouldBeFalse();
        }

        [Test]
        public void EntityTypeMappedToClrType()
        {
            inspector.EntityType.ShouldEqual(mapping.ContainingEntityType);
        }

        [Test]
        public void EntityTypeIsSet()
        {
            inspector.IsSet(Prop(x => x.EntityType))
                .ShouldBeTrue();
        }

        [Test]
        public void FormulaMapped()
        {
            mapping.Formula = "formula";
            inspector.Formula.ShouldEqual(mapping.Formula);
        }

        [Test]
        public void FormulaIsSet()
        {
            mapping.Formula = "formula";
            inspector.IsSet(Prop(x => x.Formula))
                .ShouldBeTrue();
        }

        [Test]
        public void FormulaIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Formula))
                .ShouldBeFalse();
        }

        [Test]
        public void InsertMapped()
        {
            mapping.Insert = true;
            inspector.Insert.ShouldEqual(mapping.Insert);
        }

        [Test]
        public void InsertIsSet()
        {
            mapping.Insert = true;
            inspector.IsSet(Prop(x => x.Insert))
                .ShouldBeTrue();
        }

        [Test]
        public void InsertIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Insert))
                .ShouldBeFalse();
        }

        private PropertyInfo Prop(Expression<Func<IPropertyInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }
    }
}
