using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmHbmStoredProcedureConverterTester
    {
        private IHbmConverter<StoredProcedureMapping, HbmCustomSQL> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<StoredProcedureMapping, HbmCustomSQL>>();
        }

        [Test]
        public void ShouldConvertQueryIfPopulated()
        {
            var storedProcedureMapping = new StoredProcedureMapping();
            storedProcedureMapping.Set(fluent => fluent.Query, Layer.Conventions, "update ABC");
            var convertedHbmCustomSQL = converter.Convert(storedProcedureMapping);
            convertedHbmCustomSQL.Text.ShouldEqual(new string[] { storedProcedureMapping.Query });
        }

        [Test]
        public void ShouldConvertQueryIfNotPopulated()
        {
            var storedProcedureMapping = new StoredProcedureMapping();
            // Don't set the anything on the original mapping
            var convertedHbmCustomSQL = converter.Convert(storedProcedureMapping);
            // Check that it was converted, even if nothing was set
            convertedHbmCustomSQL.Text.ShouldEqual(new string[] { storedProcedureMapping.Query });
        }

        [Test]
        public void ShouldConvertCheckIfPopulatedWithValidValue()
        {
            var check = HbmCustomSQLCheck.Rowcount; // Defaults to None, so use this to ensure that we can detect changes

            var storedProcedureMapping = new StoredProcedureMapping();
            var checkDict = new XmlLinkedEnumBiDictionary<HbmCustomSQLCheck>();
            storedProcedureMapping.Set(fluent => fluent.Check, Layer.Conventions, checkDict[check]);
            var convertedHbmCustomSQL = converter.Convert(storedProcedureMapping);
            convertedHbmCustomSQL.check.ShouldEqual(check);
            Assert.That(convertedHbmCustomSQL.checkSpecified.Equals(true), "Check was not marked as specified");
        }

        [Test]
        public void ShouldFailToConvertCheckIfPopulatedWithInvalidValue()
        {
            var storedProcedureMapping = new StoredProcedureMapping();
            storedProcedureMapping.Set(fluent => fluent.Check, Layer.Conventions, "invalid_value");
            Assert.Throws<NotSupportedException>(() => converter.Convert(storedProcedureMapping));
        }

        // This one is weird: because the default is an empty string rather than null, it is considered "specified"...
        [Test]
        public void ShouldConvertCheckIfNotPopulated()
        {
            var storedProcedureMapping = new StoredProcedureMapping();
            // Don't set the anything on the original mapping
            var convertedHbmCustomSQL = converter.Convert(storedProcedureMapping);
            var checkDict = new XmlLinkedEnumBiDictionary<HbmCustomSQLCheck>();
            convertedHbmCustomSQL.check.ShouldEqual(checkDict[storedProcedureMapping.Check]);
            Assert.That(convertedHbmCustomSQL.checkSpecified.Equals(true), "Check was not marked as specified");
        }
    }
}
