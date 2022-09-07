using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmPropertyConverterTester
    {
        private IHbmConverter<PropertyMapping, HbmProperty> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<PropertyMapping, HbmProperty>>();
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.access.ShouldEqual(propertyMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.access.ShouldEqual(blankHbmProperty.access);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.name.ShouldEqual(propertyMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.name.ShouldEqual(blankHbmProperty.name);
        }

        [Test]
        public void ShouldConvertTypeIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Type, Layer.Conventions, new TypeReference(typeof(HbmPropertyConverterTester))); // Can be any class, this one is just guaranteed to exist
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.type1.ShouldEqual(propertyMapping.Type.ToString());
        }

        [Test]
        public void ShouldNotConvertTypeIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.type1.ShouldEqual(blankHbmProperty.type1);
        }

        [Test]
        public void ShouldConvertFormulaIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Formula, Layer.Conventions, "form");
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.formula.ShouldEqual(propertyMapping.Formula);
        }

        [Test]
        public void ShouldNotConvertFormulaIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.formula.ShouldEqual(blankHbmProperty.formula);
        }

        [Test]
        public void ShouldConvertGeneratedIfPopulatedWithValidValue()
        {
            var generated = HbmPropertyGeneration.Always; // Defaults to Never, use this to ensure that we can detect changes

            var propertyMapping = new PropertyMapping();
            var genDict = new XmlLinkedEnumBiDictionary<HbmPropertyGeneration>();
            propertyMapping.Set(fluent => fluent.Generated, Layer.Conventions, genDict[generated]);
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.generated.ShouldEqual(generated);
        }

        [Test]
        public void ShouldFailToConvertGeneratedIfPopulatedWithInvalidValue()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Generated, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(propertyMapping));
        }

        [Test]
        public void ShouldNotConvertGeneratedIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.generated.ShouldEqual(blankHbmProperty.generated);
        }

        [Test]
        public void ShouldConvertInsertIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Insert, Layer.Conventions, true); // Defaults to false, so use this to ensure we can detect changes
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.insert.ShouldEqual(propertyMapping.Insert);
            Assert.That(convertedHbmProperty.insertSpecified.Equals(true), "Insert was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertInsertIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.insert.ShouldEqual(blankHbmProperty.insert);
            Assert.That(convertedHbmProperty.insertSpecified.Equals(false), "Insert was marked as specified");
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure we can detect changes
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.optimisticlock.ShouldEqual(propertyMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.optimisticlock.ShouldEqual(blankHbmProperty.optimisticlock);
        }

        [Test]
        public void ShouldConvertUpdateIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Update, Layer.Conventions, true); // Defaults to false, so use this to ensure we can detect changes
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.update.ShouldEqual(propertyMapping.Update);
            Assert.That(convertedHbmProperty.updateSpecified.Equals(true), "Update was not marked as specified");
        }

        [Test]
        public void ShouldNotConvertUpdateIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.update.ShouldEqual(blankHbmProperty.update);
            Assert.That(convertedHbmProperty.updateSpecified.Equals(false), "Update was marked as specified");
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true); // Defaults to false, so use this to ensure we can detect changes
            var convertedHbmProperty = converter.Convert(propertyMapping);
            convertedHbmProperty.lazy.ShouldEqual(propertyMapping.Lazy);
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var propertyMapping = new PropertyMapping();
            // Don't set anything on the original mapping
            var convertedHbmProperty = converter.Convert(propertyMapping);
            var blankHbmProperty = new HbmProperty();
            convertedHbmProperty.lazy.ShouldEqual(blankHbmProperty.lazy);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<PropertyMapping, ColumnMapping, HbmProperty, HbmColumn, object>(
                (propertyMapping, columnMapping) => propertyMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmProperty => hbmProperty.Items);
        }
    }
}