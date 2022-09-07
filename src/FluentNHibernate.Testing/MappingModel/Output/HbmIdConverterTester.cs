using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIdConverterTester
    {
        private IHbmConverter<IdMapping, HbmId> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<IdMapping, HbmId>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var idMapping = new IdMapping();
            idMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmId = converter.Convert(idMapping);
            convertedHbmId.access.ShouldEqual(idMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var idMapping = new IdMapping();
            // Don't set anything on the original mapping
            var convertedHbmId = converter.Convert(idMapping);
            var blankHbmId = new HbmId();
            convertedHbmId.access.ShouldEqual(blankHbmId.access);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var idMapping = new IdMapping();
            idMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmId = converter.Convert(idMapping);
            convertedHbmId.name.ShouldEqual(idMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var idMapping = new IdMapping();
            // Don't set anything on the original mapping
            var convertedHbmId = converter.Convert(idMapping);
            var blankHbmId = new HbmId();
            convertedHbmId.name.ShouldEqual(blankHbmId.name);
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var idMapping = new IdMapping();
            idMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference(typeof(HbmIdConverterTester))); // Can be any class, this one is just guaranteed to exist
            var convertedHbmId = converter.Convert(idMapping);
            convertedHbmId.type1.ShouldEqual(idMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var idMapping = new IdMapping();
            // Don't set anything on the original mapping
            var convertedHbmId = converter.Convert(idMapping);
            var blankHbmId = new HbmId();
            convertedHbmId.type1.ShouldEqual(blankHbmId.type1);
        }

        [Test]
        public void ShouldConvertUnsavedValueIfPopulated()
        {
            var idMapping = new IdMapping();
            idMapping.Set(fluent => fluent.UnsavedValue, Layer.Conventions, "u-value");
            var convertedHbmId = converter.Convert(idMapping);
            convertedHbmId.unsavedvalue.ShouldEqual(idMapping.UnsavedValue);
        }

        [Test]
        public void ShouldNotConvertUnsavedValueIfNotPopulated()
        {
            var idMapping = new IdMapping();
            // Don't set anything on the original mapping
            var convertedHbmId = converter.Convert(idMapping);
            var blankHbmId = new HbmId();
            convertedHbmId.unsavedvalue.ShouldEqual(blankHbmId.unsavedvalue);
        }

        [Test]
        public void ShouldConvertGenerator()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<IdMapping, GeneratorMapping, HbmId, HbmGenerator>(
                (idMapping, generatorMapping) => idMapping.Set(fluent => fluent.Generator, Layer.Defaults, generatorMapping),
                hbmId => hbmId.generator);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<IdMapping, ColumnMapping, HbmId, HbmColumn>(
                (idMapping, columnMapping) => idMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmId => hbmId.column);
        }
    }
}