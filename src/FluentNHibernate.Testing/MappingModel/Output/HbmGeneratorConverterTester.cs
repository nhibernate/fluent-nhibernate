using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmGeneratorConverterTester
    {
        private IHbmConverter<GeneratorMapping, HbmGenerator> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<GeneratorMapping, HbmGenerator>>();
        }

        [Test]
        public void ShouldConvertConditionIfPopulated()
        {
            var generatorMapping = new GeneratorMapping();
            generatorMapping.Set(fluent => fluent.Class, Layer.Conventions, "class");
            var convertedHbmGenerator = converter.Convert(generatorMapping);
            convertedHbmGenerator.@class.ShouldEqual(generatorMapping.Class);
        }

        [Test]
        public void ShouldNotConvertConditionIfNotPopulated()
        {
            var generatorMapping = new GeneratorMapping();
            // Don't set the schema on the original mapping
            var convertedHbmGenerator = converter.Convert(generatorMapping);
            var blankHbmGenerator = new HbmGenerator();
            convertedHbmGenerator.@class.ShouldEqual(blankHbmGenerator.@class);
        }

        [Test]
        public void ShouldNotConvertConditionIfEmpty()
        {
            var generatorMapping = new GeneratorMapping();
            generatorMapping.Set(fluent => fluent.Class, Layer.Conventions, "");
            var convertedHbmGenerator = converter.Convert(generatorMapping);
            var blankHbmGenerator = new HbmGenerator();
            convertedHbmGenerator.@class.ShouldEqual(blankHbmGenerator.@class);
        }

        [Test]
        public void ShouldConvertParameters()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("first", "value");
            parameters.Add("second", "another-value");

            var generatorMapping = new GeneratorMapping();
            foreach (var parameter in parameters)
                generatorMapping.Params.Add(parameter);

            var convertedHbmGenerator = converter.Convert(generatorMapping);

            // Since we check the actual conversion of GeneratorMapping parameters to HbmFilterParam elsewhere,
            // and dictionaries have no order but arrays do (which complicates trying to test value equality), just check
            // that we got the right number of items, here.
            convertedHbmGenerator.param.ShouldNotBeNull();
            convertedHbmGenerator.param.Length.ShouldEqual(parameters.Count);
        }

        [Test]
        public void ShouldNotConvertZeroParameters()
        {
            var generatorMapping = new GeneratorMapping();
            // Don't add any parameters
            var convertedHbmGenerator = converter.Convert(generatorMapping);
            var blankHbmGenerator = new HbmGenerator();
            convertedHbmGenerator.param.ShouldEqual(blankHbmGenerator.param);
        }
    }
}
