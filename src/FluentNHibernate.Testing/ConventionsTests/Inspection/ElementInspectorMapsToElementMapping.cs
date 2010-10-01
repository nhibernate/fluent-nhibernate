using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class ElementInspectorMapsToElementMapping
    {
        private ElementMapping mapping;
        private IElementInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new ElementMapping();
            inspector = new ElementInspector(mapping);
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
        public void FormulaMapped()
        {
            mapping.Formula = "formula";
            inspector.Formula.ShouldEqual("formula");
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
        public void LengthMapped()
        {
            mapping.Length = 50;
            inspector.Length.ShouldEqual(50);
        }

        [Test]
        public void LengthIsSet()
        {
            mapping.Length = 50;
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
        public void PrecisionMapped()
        {
            mapping.Precision = 50;
            inspector.Precision.ShouldEqual(50);
        }

        [Test]
        public void PrecisionIsSet()
        {
            mapping.Precision = 50;
            inspector.IsSet(Prop(x => x.Precision))
                .ShouldBeTrue();
        }

        [Test]
        public void PrecisionIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Precision))
                .ShouldBeFalse();
        }

        [Test]
        public void ScaleMapped()
        {
            mapping.Scale = 50;
            inspector.Scale.ShouldEqual(50);
        }

        [Test]
        public void ScaleIsSet()
        {
            mapping.Scale = 50;
            inspector.IsSet(Prop(x => x.Scale))
                .ShouldBeTrue();
        }

        [Test]
        public void ScaleIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Scale))
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
        public void UniqueMapped()
        {
            mapping.Unique = true;
            inspector.Unique.ShouldBeTrue();
        }

        [Test]
        public void UniqueIsSet()
        {
            mapping.Unique = true;
            inspector.IsSet(Prop(x => x.Unique))
                .ShouldBeTrue();
        }

        [Test]
        public void UniqueIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Unique))
                .ShouldBeFalse();
        }

        #region Helpers

        private Member Prop(Expression<Func<IElementInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetMember(propertyExpression);
        }

        #endregion
    }
}