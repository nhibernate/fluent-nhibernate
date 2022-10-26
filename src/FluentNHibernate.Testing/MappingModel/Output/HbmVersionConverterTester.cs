using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmVersionConverterTester
    {
        private IHbmConverter<VersionMapping, HbmVersion> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<VersionMapping, HbmVersion>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var versionMapping = new VersionMapping();
            versionMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmVersion = converter.Convert(versionMapping);
            convertedHbmVersion.access.ShouldEqual(versionMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var versionMapping = new VersionMapping();
            // Don't set anything on the original mapping
            var convertedHbmVersion = converter.Convert(versionMapping);
            var blankHbmVersion = new HbmVersion();
            convertedHbmVersion.access.ShouldEqual(blankHbmVersion.access);
        }

        [Test]
        public void ShouldConvertGeneratedIfPopulatedWithValidValue()
        {
            var generated = HbmVersionGeneration.Always; // Defaults to Never, so use this to ensure that we can detect changes

            var versionMapping = new VersionMapping();
            var generatedDict = new XmlLinkedEnumBiDictionary<HbmVersionGeneration>();
            versionMapping.Set(fluent => fluent.Generated, Layer.Conventions, generatedDict[generated]);
            var convertedHbmVersion = converter.Convert(versionMapping);
            convertedHbmVersion.generated.ShouldEqual(generated);
        }

        [Test]
        public void ShouldFailToConvertGeneratedIfPopulatedWithInvalidValue()
        {
            var versionMapping = new VersionMapping();
            versionMapping.Set(fluent => fluent.Generated, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(versionMapping));
        }

        [Test]
        public void ShouldNotConvertGeneratedIfNotPopulated()
        {
            var versionMapping = new VersionMapping();
            // Don't set anything on the original mapping
            var convertedHbmVersion = converter.Convert(versionMapping);
            var blankHbmVersion = new HbmVersion();
            convertedHbmVersion.generated.ShouldEqual(blankHbmVersion.generated);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var versionMapping = new VersionMapping();
            versionMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmVersion = converter.Convert(versionMapping);
            convertedHbmVersion.name.ShouldEqual(versionMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var versionMapping = new VersionMapping();
            // Don't set anything on the original mapping
            var convertedHbmVersion = converter.Convert(versionMapping);
            var blankHbmVersion = new HbmVersion();
            convertedHbmVersion.name.ShouldEqual(blankHbmVersion.name);
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var versionMapping = new VersionMapping();
            versionMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference("type"));
            var convertedHbmVersion = converter.Convert(versionMapping);
            convertedHbmVersion.type.ShouldEqual(versionMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var versionMapping = new VersionMapping();
            // Don't set anything on the original mapping
            var convertedHbmVersion = converter.Convert(versionMapping);
            var blankHbmVersion = new HbmVersion();
            convertedHbmVersion.type.ShouldEqual(blankHbmVersion.type);
        }

        [Test]
        public void ShouldConvertUnsavedValueIfPopulated()
        {
            var versionMapping = new VersionMapping();
            versionMapping.Set(fluent => fluent.UnsavedValue, Layer.Conventions, "u-value");
            var convertedHbmVersion = converter.Convert(versionMapping);
            convertedHbmVersion.unsavedvalue.ShouldEqual(versionMapping.UnsavedValue);
        }

        [Test]
        public void ShouldNotConvertUnsavedValueIfNotPopulated()
        {
            var versionMapping = new VersionMapping();
            // Don't set anything on the original mapping
            var convertedHbmVersion = converter.Convert(versionMapping);
            var blankHbmVersion = new HbmVersion();
            convertedHbmVersion.unsavedvalue.ShouldEqual(blankHbmVersion.unsavedvalue);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<VersionMapping, ColumnMapping, HbmVersion, HbmColumn>(
                (versionMapping, columnMapping) => versionMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmVersion => hbmVersion.column);
        }
    }
}