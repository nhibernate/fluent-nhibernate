using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions;
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
        public void ColumnCollectionHasSameCountAsMapping()
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
        public void LengthMapped()
        {
            mapping.AddColumn(new ColumnMapping { Length = 100 });
            inspector.Length.ShouldEqual(100);
        }

        [Test]
        public void LengthIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Length = 100 });
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
        public void PrecisionIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Precision = 10 });
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
            mapping.AddColumn(new ColumnMapping { Scale = 10 });
            inspector.Scale.ShouldEqual(10);
        }

        [Test]
        public void ScaleIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Scale = 10 });
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
        public void NullableMapped()
        {
            mapping.AddColumn(new ColumnMapping { NotNull = false });
            inspector.Nullable.ShouldEqual(true);
        }

        [Test]
        public void NullableIsSet()
        {
            mapping.AddColumn(new ColumnMapping { NotNull = false });
            inspector.IsSet(Prop(x => x.Nullable))
                .ShouldBeTrue();
        }

        [Test]
        public void NullableIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Nullable))
                .ShouldBeFalse();
        }

        [Test]
        public void UniqueMapped()
        {
            mapping.AddColumn(new ColumnMapping { Unique = true });
            inspector.Unique.ShouldEqual(true);
        }

        [Test]
        public void UniqueIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Unique = true });
            inspector.IsSet(Prop(x => x.Unique))
                .ShouldBeTrue();
        }

        [Test]
        public void UniqueIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Unique))
                .ShouldBeFalse();
        }

        [Test]
        public void UniqueKeyMapped()
        {
            mapping.AddColumn(new ColumnMapping { UniqueKey = "key" });
            inspector.UniqueKey.ShouldEqual("key");
        }

        [Test]
        public void UniqueKeyIsSet()
        {
            mapping.AddColumn(new ColumnMapping { UniqueKey = "key" });
            inspector.IsSet(Prop(x => x.UniqueKey))
                .ShouldBeTrue();
        }

        [Test]
        public void UniqueKeyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.UniqueKey))
                .ShouldBeFalse();
        }

        [Test]
        public void SqlTypeMapped()
        {
            mapping.AddColumn(new ColumnMapping { SqlType = "sql" });
            inspector.SqlType.ShouldEqual("sql");
        }

        [Test]
        public void SqlTypeIsSet()
        {
            mapping.AddColumn(new ColumnMapping { SqlType = "sql" });
            inspector.IsSet(Prop(x => x.SqlType))
                .ShouldBeTrue();
        }

        [Test]
        public void SqlTypeIsNotSet()
        {
            inspector.IsSet(Prop(x => x.SqlType))
                .ShouldBeFalse();
        }

        [Test]
        public void IndexMapped()
        {
            mapping.AddColumn(new ColumnMapping { Index = "index" });
            inspector.Index.ShouldEqual("index");
        }

        [Test]
        public void IndexIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Index = "index" });
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
        public void CheckMapped()
        {
            mapping.AddColumn(new ColumnMapping { Check = "key" });
            inspector.Check.ShouldEqual("key");
        }

        [Test]
        public void CheckIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Check = "key" });
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
        public void DefaultMapped()
        {
            mapping.AddColumn(new ColumnMapping { Default = "key" });
            inspector.Default.ShouldEqual("key");
        }

        [Test]
        public void DefaultIsSet()
        {
            mapping.AddColumn(new ColumnMapping { Default = "key" });
            inspector.IsSet(Prop(x => x.Default))
                .ShouldBeTrue();
        }

        [Test]
        public void DefaultIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Default))
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