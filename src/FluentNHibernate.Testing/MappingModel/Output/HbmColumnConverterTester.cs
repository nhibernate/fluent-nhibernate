using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmColumnConverterTester
    {
        private IHbmConverter<ColumnMapping, HbmColumn> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ColumnMapping, HbmColumn>>();
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Check, Layer.Conventions, "ck");
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.check.ShouldEqual(columnMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.check.ShouldEqual(blankHbmColumn.check);
        }

        [Test]
        public void ShouldConvertDefaultIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Default, Layer.Conventions, "df");
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.@default.ShouldEqual(columnMapping.Default);
        }

        [Test]
        public void ShouldNotConvertDefaultIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.@default.ShouldEqual(blankHbmColumn.@default);
        }

        [Test]
        public void ShouldConvertIndexIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Index, Layer.Conventions, "ix");
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.index.ShouldEqual(columnMapping.Index);
        }

        [Test]
        public void ShouldNotConvertIndexIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.index.ShouldEqual(blankHbmColumn.index);
        }

        [Test]
        public void ShouldConvertLengthIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Length, Layer.Conventions, 10);
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.length.ShouldEqual(columnMapping.Length.ToString());
        }

        [Test]
        public void ShouldNotConvertLengthIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.length.ShouldEqual(blankHbmColumn.length);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.name.ShouldEqual(columnMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.name.ShouldEqual(blankHbmColumn.name);
        }

        [Test]
        public void ShouldConvertNotNullIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.NotNull, Layer.Conventions, true); // Defaults to false, so use true so that we can detect changes
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.notnull.ShouldEqual(columnMapping.NotNull);
            Assert.That(convertedHbmColumn.notnullSpecified.Equals(true), "NotNull was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertNotNullIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.notnull.ShouldEqual(blankHbmColumn.notnull);
            Assert.That(convertedHbmColumn.notnullSpecified.Equals(false), "NotNull was marked as specified");
        }

        [Test]
        public void ShouldConvertSqlTypeIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.SqlType, Layer.Conventions, "type"); // FIXME: Is this a realistic value?
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.sqltype.ShouldEqual(columnMapping.SqlType);
        }

        [Test]
        public void ShouldNotConvertSqlTypeIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.sqltype.ShouldEqual(blankHbmColumn.sqltype);
        }

        [Test]
        public void ShouldConvertUniqueIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Unique, Layer.Conventions, true); // Defaults to false, so use true so that we can detect changes
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.unique.ShouldEqual(columnMapping.Unique);
            Assert.That(convertedHbmColumn.uniqueSpecified.Equals(true), "Unique was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertUniqueIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.unique.ShouldEqual(blankHbmColumn.unique);
            Assert.That(convertedHbmColumn.uniqueSpecified.Equals(false), "Unique was marked as specified");
        }

        [Test]
        public void ShouldConvertUniqueKeyIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.UniqueKey, Layer.Conventions, "uk");
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.uniquekey.ShouldEqual(columnMapping.UniqueKey);
        }

        [Test]
        public void ShouldNotConvertUniqueKeyIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.uniquekey.ShouldEqual(blankHbmColumn.uniquekey);
        }

        [Test]
        public void ShouldConvertPrecisionIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Precision, Layer.Conventions, 10);
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.precision.ShouldEqual(columnMapping.Precision.ToString());
        }

        [Test]
        public void ShouldNotConvertPrecisionIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.precision.ShouldEqual(blankHbmColumn.precision);
        }

        [Test]
        public void ShouldConvertScaleIfPopulated()
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(fluent => fluent.Scale, Layer.Conventions, 10);
            var convertedHbmColumn = converter.Convert(columnMapping);
            convertedHbmColumn.scale.ShouldEqual(columnMapping.Scale.ToString());
        }

        [Test]
        public void ShouldNotConvertScaleIfNotPopulated()
        {
            var columnMapping = new ColumnMapping();
            // Don't set anything on the original mapping
            var convertedHbmColumn = converter.Convert(columnMapping);
            var blankHbmColumn = new HbmColumn();
            convertedHbmColumn.scale.ShouldEqual(blankHbmColumn.scale);
        }
    }
}