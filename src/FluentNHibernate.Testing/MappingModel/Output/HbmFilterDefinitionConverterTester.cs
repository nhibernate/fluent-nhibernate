using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmFilterDefinitionConverterTester
    {
        private static KeyValuePair<string, IType> SAMPLE_FILTER_PARAM_1 = new KeyValuePair<string, IType>("george", NHibernateUtil.Int32);
        private static KeyValuePair<string, IType> SAMPLE_FILTER_PARAM_2 = new KeyValuePair<string, IType>("fred", NHibernateUtil.Guid);

        private IHbmConverter<FilterDefinitionMapping, HbmFilterDef> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<FilterDefinitionMapping, HbmFilterDef>>();
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var filterDefinitionMapping = new FilterDefinitionMapping();
            filterDefinitionMapping.Set(fluent => fluent.Name, Layer.Conventions, "sid");
            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);
            convertedHbmFilterDef.name.ShouldEqual(filterDefinitionMapping.Name);
        }

        [Test]
        public void ShouldConvertNameIfNotPopulated()
        {
            var filterDefinitionMapping = new FilterDefinitionMapping();
            // Don't set the schema on the original mapping
            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);
            convertedHbmFilterDef.name.ShouldEqual(filterDefinitionMapping.Name);
        }

        [Test]
        public void ShouldConvertConditionIfPopulated()
        {
            var filterDefinitionMapping = new FilterDefinitionMapping();
            filterDefinitionMapping.Set(fluent => fluent.Condition, Layer.Conventions, "1=1");
            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);
            convertedHbmFilterDef.condition.ShouldEqual(filterDefinitionMapping.Condition);
        }

        [Test]
        public void ShouldNotConvertConditionIfNotPopulated()
        {
            var filterDefinitionMapping = new FilterDefinitionMapping();
            // Don't set the schema on the original mapping
            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);
            var blankHbmFilterDef = new HbmFilterDef();
            convertedHbmFilterDef.condition.ShouldEqual(blankHbmFilterDef.condition);
        }

        [Test]
        public void ShouldNotConvertConditionIfEmpty()
        {
            var filterDefinitionMapping = new FilterDefinitionMapping();
            filterDefinitionMapping.Set(fluent => fluent.Condition, Layer.Conventions, "");
            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);
            var blankHbmFilterDef = new HbmFilterDef();
            convertedHbmFilterDef.condition.ShouldEqual(blankHbmFilterDef.condition);
        }
        
        [Test]
        public void ShouldConvertParameters()
        {
            IDictionary<string, IType> parameters = new Dictionary<string, IType>();
            parameters.Add(SAMPLE_FILTER_PARAM_1);
            parameters.Add(SAMPLE_FILTER_PARAM_2);

            var filterDefinitionMapping = new FilterDefinitionMapping();
            foreach (var parameter in parameters)
                filterDefinitionMapping.Parameters.Add(parameter);

            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);

            // Since we check the actual conversion of FilterDefinitionMapping parameters to HbmFilterParam elsewhere,
            // and dictionaries have no order but arrays do (which complicates trying to test value equality), just check
            // that we got the right number of items, here.
            convertedHbmFilterDef.Items.ShouldNotBeNull();
            convertedHbmFilterDef.Items.Length.ShouldEqual(parameters.Count);
        }

        [Test]
        public void ShouldNotConvertZeroParameters()
        {
            var filterDefinitionMapping = new FilterDefinitionMapping();
            // Don't add any parameters
            var convertedHbmFilterDef = converter.Convert(filterDefinitionMapping);
            var blankHbmFilterDef = new HbmFilterDef();
            convertedHbmFilterDef.Items.ShouldEqual(blankHbmFilterDef.Items);
        }
    }
}
