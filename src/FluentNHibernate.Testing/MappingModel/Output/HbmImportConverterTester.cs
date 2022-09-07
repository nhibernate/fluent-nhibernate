using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmImportConverterTester
    {
        private IHbmConverter<ImportMapping, HbmImport> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ImportMapping, HbmImport>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var importMapping = new ImportMapping();
            importMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference(typeof(HbmImportConverterTester))); // Can be any class, this one is just guaranteed to exist
            var convertedHbmImport = converter.Convert(importMapping);
            convertedHbmImport.@class.ShouldEqual(importMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var importMapping = new ImportMapping();
            // Don't set anything on the original mapping
            var convertedHbmImport = converter.Convert(importMapping);
            var blankHbmImport = new HbmImport();
            convertedHbmImport.@class.ShouldEqual(blankHbmImport.@class);
        }

        [Test]
        public void ShouldConvertRenameIfPopulated()
        {
            var importMapping = new ImportMapping();
            importMapping.Set(fluent => fluent.Rename, Layer.Conventions, "renamed");
            var convertedHbmImport = converter.Convert(importMapping);
            convertedHbmImport.rename.ShouldEqual(importMapping.Rename);
        }

        [Test]
        public void ShouldNotConvertRenameIfNotPopulated()
        {
            var importMapping = new ImportMapping();
            // Don't set anything on the original mapping
            var convertedHbmImport = converter.Convert(importMapping);
            var blankHbmImport = new HbmImport();
            convertedHbmImport.rename.ShouldEqual(blankHbmImport.rename);
        }
    }
}