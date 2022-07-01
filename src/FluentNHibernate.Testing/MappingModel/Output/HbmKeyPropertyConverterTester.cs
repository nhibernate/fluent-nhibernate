using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmKeyPropertyConverterTester
    {
        private IHbmConverter<KeyPropertyMapping, HbmKeyProperty> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<KeyPropertyMapping, HbmKeyProperty>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            keyPropertyMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            convertedHbmKeyProperty.access.ShouldEqual(keyPropertyMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            var blankHbmKeyProperty = new HbmKeyProperty();
            convertedHbmKeyProperty.access.ShouldEqual(blankHbmKeyProperty.access);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            keyPropertyMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            convertedHbmKeyProperty.name.ShouldEqual(keyPropertyMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            var blankHbmKeyProperty = new HbmKeyProperty();
            convertedHbmKeyProperty.name.ShouldEqual(blankHbmKeyProperty.name);
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            keyPropertyMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference(typeof(HbmKeyPropertyConverterTester))); // Can be any class, this one is just guaranteed to exist
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            convertedHbmKeyProperty.type1.ShouldEqual(keyPropertyMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            var blankHbmKeyProperty = new HbmKeyProperty();
            convertedHbmKeyProperty.type1.ShouldEqual(blankHbmKeyProperty.type1);
        }

        // If the length attribute is set, but no columns are present, convert it normally
        [Test]
        public void ShouldConvertLengthIfPopulatedWithoutColumns()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            keyPropertyMapping.Set(fluent => fluent.Length, Layer.Conventions, 8);
            // Don't add any columns
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            convertedHbmKeyProperty.length.ShouldEqual(keyPropertyMapping.Length.ToString());
        }

        [Test]
        public void ShouldNotConvertLengthIfPopulatedWithColumns()
        {
            var keyPropertyMappingLength = 8;
            var columnMappingLength = -1;

            // Set up the key property with a length and a mix of columns that do/don't have their own length
            var keyPropertyMapping = new KeyPropertyMapping();
            keyPropertyMapping.Set(fluent => fluent.Length, Layer.Conventions, keyPropertyMappingLength);
            var columnMappingWithLength = new ColumnMapping();
            columnMappingWithLength.Set(fluent => fluent.Length, Layer.Conventions, columnMappingLength);
            keyPropertyMapping.AddColumn(columnMappingWithLength);
            var columnMappingWithoutLength = new ColumnMapping();
            keyPropertyMapping.AddColumn(columnMappingWithoutLength);

            // Run the converter
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);

            // Check that the Length value wasn't propagated to the HbmKeyProperty
            var blankHbmKeyProperty = new HbmKeyProperty();
            convertedHbmKeyProperty.length.ShouldEqual(blankHbmKeyProperty.length);

            // Check that the length property _was_ propagated to the columns, but only those which did not already have a length value
            // NOTE: This covers Bug #231 (which has a separate test in the XML variant)
            columnMappingWithLength.Length.ShouldEqual(columnMappingLength);
            columnMappingWithoutLength.Length.ShouldEqual(keyPropertyMappingLength);
        }

        // If the length attribute is not set, do not convert it
        [Test]
        public void ShouldNotConvertLengthIfNotPopulated()
        {
            var keyPropertyMapping = new KeyPropertyMapping();
            // Don't set anything on the original mapping
            // Also don't add any columns
            var convertedHbmKeyProperty = converter.Convert(keyPropertyMapping);
            var blankHbmKeyProperty = new HbmKeyProperty();
            convertedHbmKeyProperty.length.ShouldEqual(blankHbmKeyProperty.length);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<KeyPropertyMapping, ColumnMapping, HbmKeyProperty, HbmColumn>(
                (keyPropertyMapping, columnMapping) => keyPropertyMapping.AddColumn(columnMapping),
                hbmKeyProperty => hbmKeyProperty.column);
        }
    }
}