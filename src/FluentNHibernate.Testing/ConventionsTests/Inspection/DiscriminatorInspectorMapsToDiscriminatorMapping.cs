using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class DiscriminatorInspectorMapsToDiscriminatorMapping
    {
        private DiscriminatorMapping mapping;
        private IDiscriminatorInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new DiscriminatorMapping();
            inspector = new DiscriminatorInspector(mapping);
        }

        [Test]
        public void ColumnMapped()
        {
            mapping.Column = "name";
            inspector.Column.ShouldEqual("name");
        }

        [Test]
        public void ColumnIsSet()
        {
            mapping.Column = "name";
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
        public void ForceMapped()
        {
            mapping.Force = true;
            inspector.Force.ShouldBeTrue();
        }

        [Test]
        public void ForceIsSet()
        {
            mapping.Force = true;
            inspector.IsSet(Prop(x => x.Force))
                .ShouldBeTrue();
        }

        [Test]
        public void ForceIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Force))
                .ShouldBeFalse();
        }

        [Test]
        public void FormulaMapped()
        {
            mapping.Formula = "e=mc^2";
            inspector.Formula.ShouldEqual("e=mc^2");
        }

        [Test]
        public void FormulaIsSet()
        {
            mapping.Formula = "e=mc^2";
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
            inspector.Insert.ShouldBeTrue();
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

        [Test]
        public void LengthMapped()
        {
            mapping.Length = 100;
            inspector.Length.ShouldEqual(100);
        }

        [Test]
        public void LengthIsSet()
        {
            mapping.Length = 100;
            inspector.IsSet(Prop(x => x.Length))
                .ShouldBeTrue();
        }

        [Test]
        public void LengthIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Length))
                .ShouldBeFalse();
        }

        [Test]
        public void NotNullMapped()
        {
            mapping.NotNull = true;
            inspector.NotNull.ShouldBeTrue();
        }

        [Test]
        public void NotNullIsSet()
        {
            mapping.NotNull = true;
            inspector.IsSet(Prop(x => x.NotNull))
                .ShouldBeTrue();
        }

        [Test]
        public void NotNullIsNotSet()
        {
            inspector.IsSet(Prop(x => x.NotNull))
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

        #region Helpers

        private PropertyInfo Prop(Expression<Func<IDiscriminatorInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }

        #endregion
    }
}