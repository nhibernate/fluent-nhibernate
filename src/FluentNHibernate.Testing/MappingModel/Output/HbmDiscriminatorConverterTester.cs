using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmDiscriminatorConverterTester
    {
        private IHbmConverter<DiscriminatorMapping, HbmDiscriminator> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<DiscriminatorMapping, HbmDiscriminator>>();
        }

        [Test]
        public void ShouldConvertForceIfPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            discriminatorMapping.Set(fluent => fluent.Force, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            convertedHbmDiscriminator.force.ShouldEqual(discriminatorMapping.Force);
        }

        [Test]
        public void ShouldNotConvertForceIfNotPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            // Don't set anything on the original mapping
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            var blankHbmDiscriminator = new HbmDiscriminator();
            convertedHbmDiscriminator.force.ShouldEqual(blankHbmDiscriminator.force);
        }

        [Test]
        public void ShouldConvertInsertIfPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            discriminatorMapping.Set(fluent => fluent.Insert, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            convertedHbmDiscriminator.insert.ShouldEqual(discriminatorMapping.Insert);
        }

        [Test]
        public void ShouldNotConvertInsertIfNotPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            // Don't set anything on the original mapping
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            var blankHbmDiscriminator = new HbmDiscriminator();
            convertedHbmDiscriminator.insert.ShouldEqual(blankHbmDiscriminator.insert);
        }

        [Test]
        public void ShouldConvertFormulaIfPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            discriminatorMapping.Set(fluent => fluent.Formula, Layer.Conventions, "f");
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            convertedHbmDiscriminator.formula.ShouldEqual(discriminatorMapping.Formula);
        }

        [Test]
        public void ShouldNotConvertFormulaIfNotPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            // Don't set anything on the original mapping
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            var blankHbmDiscriminator = new HbmDiscriminator();
            convertedHbmDiscriminator.formula.ShouldEqual(blankHbmDiscriminator.formula);
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var typeRef = new TypeReference(typeof(HbmDiscriminatorConverterTester));

            var discriminatorMapping = new DiscriminatorMapping();
            discriminatorMapping.Set(fluent => fluent.Type, Layer.Conventions, typeRef); // Can be any class, this one is just guaranteed to exist
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            convertedHbmDiscriminator.type.ShouldEqual(TypeMapping.GetTypeString(typeRef.GetUnderlyingSystemType()));
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var discriminatorMapping = new DiscriminatorMapping();
            // Don't set anything on the original mapping
            var convertedHbmDiscriminator = converter.Convert(discriminatorMapping);
            var blankHbmDiscriminator = new HbmDiscriminator();
            convertedHbmDiscriminator.type.ShouldEqual(blankHbmDiscriminator.type);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectAsLooselyTypedField<DiscriminatorMapping, ColumnMapping, HbmDiscriminator, HbmColumn, object>(
                (discriminatorMapping, columnMapping) => discriminatorMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmDiscriminator => hbmDiscriminator.Item);
        }
    }
}