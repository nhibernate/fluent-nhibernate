using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIndexConverterTester
    {
        private IHbmConverter<IndexMapping, HbmIndex> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<IndexMapping, HbmIndex>>();
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var indexMapping = new IndexMapping();
            indexMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference("type"));
            var convertedHbmIndex = converter.Convert(indexMapping);
            convertedHbmIndex.type.ShouldEqual(indexMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var indexMapping = new IndexMapping();
            // Don't set anything on the original mapping
            var convertedHbmIndex = converter.Convert(indexMapping);
            var blankHbmIndex = new HbmIndex();
            convertedHbmIndex.type.ShouldEqual(blankHbmIndex.type);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<IndexMapping, ColumnMapping, HbmIndex, HbmColumn>(
                (indexMapping, columnMapping) => indexMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmIndex => hbmIndex.column);
        }
    }
}