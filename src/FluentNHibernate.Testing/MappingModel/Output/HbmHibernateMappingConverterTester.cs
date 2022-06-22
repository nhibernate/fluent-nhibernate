using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmHibernateMappingConverterTester
    {
        private IHbmConverter<HibernateMapping, HbmMapping> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<HibernateMapping, HbmMapping>>();
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.schema.ShouldEqual(hibernateMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.schema.ShouldEqual(blankHbmMapping.schema);
        }

        [Test]
        public void ShouldConvertDefaultCascadeIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.DefaultCascade, Layer.Conventions, "cas");
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.defaultcascade.ShouldEqual(hibernateMapping.DefaultCascade);
        }

        [Test]
        public void ShouldNotConvertDefaultCascadeIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.defaultcascade.ShouldEqual(blankHbmMapping.defaultcascade);
        }

        [Test]
        public void ShouldConvertDefaultAccessIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.DefaultAccess, Layer.Conventions, "acc");
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.defaultaccess.ShouldEqual(hibernateMapping.DefaultAccess);
        }

        [Test]
        public void ShouldNotConvertDefaultAccessIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.defaultaccess.ShouldEqual(blankHbmMapping.defaultaccess);
        }

        [Test]
        public void ShouldConvertAutoImportIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.AutoImport, Layer.Conventions, false); // Defaults to true, so specify false in order to ensure that we can tell if it was set
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.autoimport.ShouldEqual(hibernateMapping.AutoImport);
        }

        [Test]
        public void ShouldNotConvertAutoImportIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.autoimport.ShouldEqual(blankHbmMapping.autoimport);
        }

        [Test]
        public void ShouldConvertDefaultLazyIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.DefaultLazy, Layer.Conventions, false); // Defaults to true, so specify false in order to ensure that we can tell if it was set
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.defaultlazy.ShouldEqual(hibernateMapping.DefaultLazy);
        }

        [Test]
        public void ShouldNotConvertDefaultLazyIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.defaultlazy.ShouldEqual(blankHbmMapping.defaultlazy);
        }

        [Test]
        public void ShouldConvertCatalogIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.Catalog, Layer.Conventions, "catalog");
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.catalog.ShouldEqual(hibernateMapping.Catalog);
        }

        [Test]
        public void ShouldNotConvertCatalogIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.catalog.ShouldEqual(blankHbmMapping.catalog);
        }

        [Test]
        public void ShouldConvertNamespaceIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.Namespace, Layer.Conventions, "namespace");
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.@namespace.ShouldEqual(hibernateMapping.Namespace);
        }

        [Test]
        public void ShouldNotConvertNamespaceIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.@namespace.ShouldEqual(blankHbmMapping.@namespace);
        }

        [Test]
        public void ShouldConvertAssemblyIfPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            hibernateMapping.Set(fluent => fluent.Assembly, Layer.Conventions, "assembly");
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            convertedHbmMapping.assembly.ShouldEqual(hibernateMapping.Assembly);
        }

        [Test]
        public void ShouldNotConvertAssemblyIfNotPopulated()
        {
            var hibernateMapping = new HibernateMapping();
            // Don't set the schema on the original mapping
            var convertedHbmMapping = converter.Convert(hibernateMapping);
            var blankHbmMapping = new HbmMapping();
            convertedHbmMapping.assembly.ShouldEqual(blankHbmMapping.assembly);
        }

        [Test]
        public void ShouldConvertImports()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<HibernateMapping, ImportMapping, HbmMapping, HbmImport>(
                (hibernateMapping, importMapping) => hibernateMapping.AddImport(importMapping),
                hbmMapping => hbmMapping.import);
        }

        // NOTE: This test implicitly checks that only one class is converted, as long as only one class is added to the original mapping
        [Test]
        public void ShouldConvertClasses()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<HibernateMapping, ClassMapping, HbmMapping, HbmClass, object>(
                (hibernateMapping, classMapping) => hibernateMapping.AddClass(classMapping),
                hbmMapping => hbmMapping.Items);
        }

        [Test]
        public void ShouldConvertFilterDefs()
        {
            ShouldConvertSubobjectsAsStrictlyTypedArray<HibernateMapping, FilterDefinitionMapping, HbmMapping, HbmFilterDef>(
                (hibernateMapping, filterDefinitionMapping) => hibernateMapping.AddFilter(filterDefinitionMapping),
                hbmMapping => hbmMapping.filterdef);
        }
    }
}