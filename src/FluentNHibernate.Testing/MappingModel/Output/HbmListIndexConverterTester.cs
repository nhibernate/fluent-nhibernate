using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmListIndexConverterTester
    {
        private IHbmConverter<IndexMapping, HbmListIndex> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<IndexMapping, HbmListIndex>>();
        }

        [Test]
        public void ShouldConvertOffsetIfPopulated()
        {
            var indexMapping = new IndexMapping();
            indexMapping.Set(fluent => fluent.Offset, Layer.Conventions, 31);
            var convertedHbmListIndex = converter.Convert(indexMapping);
            convertedHbmListIndex.@base.ShouldEqual(indexMapping.Offset.ToString());
        }

        // Offset is always populated, since it is a precondition of reaching the list index converter

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectAsStrictlyTypedField<IndexMapping, ColumnMapping, HbmListIndex, HbmColumn>(
                (indexMapping, columnMapping) => indexMapping.AddColumn(Layer.Conventions, columnMapping),
                hbmListIndex => hbmListIndex.column);
        }
    }
}