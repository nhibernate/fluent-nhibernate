using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Inspection
{
    [TestFixture, Category("Inspection DSL")]
    public class ColumnInspectorMapsToColumnMapping
    {
        private ColumnMapping mapping;
        private IColumnInspector inspector;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new ColumnMapping();
            inspector = new ColumnInspector(typeof(Record), mapping);
        }

        [Test]
        public void CheckMapped()
        {
            mapping.Check = "chk";
            inspector.Check.ShouldEqual("chk");
        }

        [Test]
        public void CheckIsSet()
        {
            mapping.Check = "chk";
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
            mapping.Default = "value";
            inspector.Default.ShouldEqual("value");
        }

        [Test]
        public void DefaultIsSet()
        {
            mapping.Default = "value";
            inspector.IsSet(Prop(x => x.Default))
                .ShouldBeTrue();
        }

        [Test]
        public void DefaultIsNotSet()
        {
            inspector.IsSet(Prop(x => x.Default))
                .ShouldBeFalse();
        }

        [Test]
        public void IndexMapped()
        {
            mapping.Index = "ix";
            inspector.Index.ShouldEqual("ix");
        }

        [Test]
        public void IndexIsSet()
        {
            mapping.Index = "ix";
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
        public void PrecisionMapped()
        {
            mapping.Precision = 10;
            inspector.Precision.ShouldEqual(10);
        }

        [Test]
        public void PrecisionIsSet()
        {
            mapping.Precision = 10;
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
            mapping.Scale = 10;
            inspector.Scale.ShouldEqual(10);
        }

        [Test]
        public void ScaleIsSet()
        {
            mapping.Scale = 10;
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
        public void SqlTypeMapped()
        {
            mapping.SqlType = "type";
            inspector.SqlType.ShouldEqual("type");
        }

        [Test]
        public void SqlTypeIsSet()
        {
            mapping.SqlType = "type";
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

        [Test]
        public void UniqueKeyMapped()
        {
            mapping.UniqueKey = "key";
            inspector.UniqueKey.ShouldEqual("key");
        }

        [Test]
        public void UniqueKeyIsSet()
        {
            mapping.UniqueKey = "key";
            inspector.IsSet(Prop(x => x.UniqueKey))
                .ShouldBeTrue();
        }

        [Test]
        public void UniqueKeyIsNotSet()
        {
            inspector.IsSet(Prop(x => x.UniqueKey))
                .ShouldBeFalse();
        }

        private PropertyInfo Prop(Expression<Func<IColumnInspector, object>> propertyExpression)
        {
            return ReflectionHelper.GetProperty(propertyExpression);
        }
    }
}