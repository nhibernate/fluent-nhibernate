using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmAnyConverterTester
    {
        private IHbmConverter<AnyMapping, HbmAny> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<AnyMapping, HbmAny>>();
        }

        [Test]
        public void ShouldConvertIdTypeIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.IdType, Layer.Conventions, "id");
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.idtype.ShouldEqual(anyMapping.IdType);
        }

        [Test]
        public void ShouldNotConvertIdTypeIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.idtype.ShouldEqual(blankHbmAny.idtype);
        }

        [Test]
        public void ShouldConvertMetaTypeIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.MetaType, Layer.Conventions, new TypeReference("meta"));
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.metatype.ShouldEqual(anyMapping.MetaType.ToString());
        }

        [Test]
        public void ShouldNotConvertMetaTypeIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.metatype.ShouldEqual(blankHbmAny.metatype);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.Name, Layer.Conventions, "nm");
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.name.ShouldEqual(anyMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.name.ShouldEqual(blankHbmAny.name);
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.Access, Layer.Conventions, "access");
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.access.ShouldEqual(anyMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.access.ShouldEqual(blankHbmAny.access);
        }

        [Test]
        public void ShouldConvertInsertIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.Insert, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.insert.ShouldEqual(anyMapping.Insert);
        }

        [Test]
        public void ShouldNotConvertInsertIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.insert.ShouldEqual(blankHbmAny.insert);
        }

        [Test]
        public void ShouldConvertUpdateIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.Update, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.update.ShouldEqual(anyMapping.Update);
        }

        [Test]
        public void ShouldNotConvertUpdateIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.update.ShouldEqual(blankHbmAny.update);
        }

        [Test]
        public void ShouldConvertCascadeIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.Cascade, Layer.Conventions, "all");
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.cascade.ShouldEqual(anyMapping.Cascade);
        }

        [Test]
        public void ShouldNotConvertCascadeIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.cascade.ShouldEqual(blankHbmAny.cascade);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true); // Defaults to false, so use this to ensure that we can detect changes
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.lazy.ShouldEqual(anyMapping.Lazy);
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.lazy.ShouldEqual(blankHbmAny.lazy);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var anyMapping = new AnyMapping();
            anyMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, false); // Defaults to true, so use this to ensure that we can detect changes
            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.optimisticlock.ShouldEqual(anyMapping.OptimisticLock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var anyMapping = new AnyMapping();
            // Don't set anything on the original mapping
            var convertedHbmAny = converter.Convert(anyMapping);
            var blankHbmAny = new HbmAny();
            convertedHbmAny.optimisticlock.ShouldEqual(blankHbmAny.optimisticlock);
        }

        [Test]
        public void ShouldConvertTypeColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<AnyMapping, ColumnMapping, HbmAny, HbmColumn, object>(
                () => new ColumnMapping("typeColumn"),
                (anyMapping, columnMapping) => anyMapping.AddTypeColumn(Layer.Conventions, columnMapping),
                hbmAny => hbmAny.column);
        }

        [Test]
        public void ShouldConvertIdentifierColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<AnyMapping, ColumnMapping, HbmAny, HbmColumn, object>(
                () => new ColumnMapping("idColumn"),
                (anyMapping, columnMapping) => anyMapping.AddIdentifierColumn(Layer.Conventions, columnMapping),
                hbmAny => hbmAny.column);
        }

        [Test]
        public void ShouldConvertTypeColumnsBeforeIdentifierColumns()
        {
            // NOTE: Unlike most subobject conversion tests, this test needs to use a real column converter rather than a mock,
            // so that the converted columns can be properly identified from their names (in order to determine whether they
            // come through in the correct order). While it would be possible to write a fake converter that would do that, it
            // wouldn't really be significantly simpler than the real implementation, so it isn't really worth the bother.
            //
            // NOTE: This test probably really doesn't belong in this location, since the logic involved is actually controlled
            // by the visiting order (when visited, the column mapping handler has no idea whether any given column is from the
            // identifier column or type column sets). Keeping it here for now largely because it exists on the XML variant and
            // we need to be sure that _something_ is testing for it.

            var anyMapping = new AnyMapping();

            // Set up interleaved ID and type columns and list at least one ID column first, to make it as difficult as possible to get right.
            // Note that we don't mis-order the "1"/"2" variants because there is no natural ordering to either type of column, so simply
            // preserving the order they were added in is the best option.
            anyMapping.AddIdentifierColumn(Layer.Defaults, new ColumnMapping("idColumn1"));
            anyMapping.AddTypeColumn(Layer.Defaults, new ColumnMapping("typeColumn1"));
            anyMapping.AddIdentifierColumn(Layer.Defaults, new ColumnMapping("idColumn2"));
            anyMapping.AddTypeColumn(Layer.Defaults, new ColumnMapping("typeColumn2"));

            var convertedHbmAny = converter.Convert(anyMapping);
            convertedHbmAny.column.Select(any => any.name).ToList().ShouldEqual(new List<string> { "typeColumn1", "typeColumn2", "idColumn1", "idColumn2" });
        }

        [Test]
        public void ShouldConvertMetaValues()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<AnyMapping, MetaValueMapping, HbmAny, HbmMetaValue, object>(
                (anyMapping, metaValueMapping) => anyMapping.AddMetaValue(metaValueMapping),
                hbmAny => hbmAny.metavalue);
        }
    }
}