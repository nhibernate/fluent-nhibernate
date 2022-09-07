using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmFilterConverterTester
    {
        private IHbmConverter<FilterMapping, HbmFilter> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<FilterMapping, HbmFilter>>();
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var filterMapping = new FilterMapping();
            filterMapping.Set(fluent => fluent.Name, Layer.Conventions, "sid");
            var convertedHbmFilter = converter.Convert(filterMapping);
            convertedHbmFilter.name.ShouldEqual(filterMapping.Name);
        }

        [Test]
        public void ShouldConvertNameIfNotPopulated()
        {
            var filterMapping = new FilterMapping();
            // Don't set anything on the original mapping
            var convertedHbmFilter = converter.Convert(filterMapping);
            convertedHbmFilter.name.ShouldEqual(filterMapping.Name);
        }

        [Test]
        public void ShouldConvertConditionIfPopulated()
        {
            var filterMapping = new FilterMapping();
            filterMapping.Set(fluent => fluent.Condition, Layer.Conventions, "Fred = :george");
            var convertedHbmFilter = converter.Convert(filterMapping);
            convertedHbmFilter.condition.ShouldEqual(filterMapping.Condition);
        }

        [Test]
        public void ShouldNotConvertConditionIfNotPopulated()
        {
            var filterMapping = new FilterMapping();
            // Don't set anything on the original mapping
            var convertedHbmFilter = converter.Convert(filterMapping);
            var blankHbmFilter = new HbmFilter();
            convertedHbmFilter.condition.ShouldEqual(blankHbmFilter.condition);
        }

        [Test]
        public void ShouldNotConvertConditionIfEmpty()
        {
            var filterMapping = new FilterMapping();
            filterMapping.Set(fluent => fluent.Condition, Layer.Conventions, "");
            var convertedHbmFilter = converter.Convert(filterMapping);
            var blankHbmFilter = new HbmFilter();
            convertedHbmFilter.condition.ShouldEqual(blankHbmFilter.condition);
        }
    }
}
