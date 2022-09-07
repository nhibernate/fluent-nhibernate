using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIndexManyToManyConverterTester
    {
        private IHbmConverter<IndexManyToManyMapping, HbmIndexManyToMany> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<IndexManyToManyMapping, HbmIndexManyToMany>>();
        }

        [Test]
        public void ShouldConvertClassIfPopulated()
        {
            var indexManyToManyMapping = new IndexManyToManyMapping();
            indexManyToManyMapping.Set(fluent => fluent.Class, Layer.Conventions, new TypeReference("cls"));
            var convertedHbmIndexManyToMany = converter.Convert(indexManyToManyMapping);
            convertedHbmIndexManyToMany.@class.ShouldEqual(indexManyToManyMapping.Class.ToString());
        }

        [Test]
        public void ShouldNotConvertClassIfNotPopulated()
        {
            var indexManyToManyMapping = new IndexManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmIndexManyToMany = converter.Convert(indexManyToManyMapping);
            var blankHbmIndexManyToMany = new HbmIndexManyToMany();
            convertedHbmIndexManyToMany.@class.ShouldEqual(blankHbmIndexManyToMany.@class);
        }

        [Test]
        public void ShouldConvertForeignKeyIfPopulated()
        {
            var indexManyToManyMapping = new IndexManyToManyMapping();
            indexManyToManyMapping.Set(fluent => fluent.ForeignKey, Layer.Conventions, "FKTest");
            var convertedHbmIndexManyToMany = converter.Convert(indexManyToManyMapping);
            convertedHbmIndexManyToMany.foreignkey.ShouldEqual(indexManyToManyMapping.ForeignKey);
        }

        [Test]
        public void ShouldNotConvertForeignKeyIfNotPopulated()
        {
            var indexManyToManyMapping = new IndexManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmIndexManyToMany = converter.Convert(indexManyToManyMapping);
            var blankHbmIndexManyToMany = new HbmIndexManyToMany();
            convertedHbmIndexManyToMany.foreignkey.ShouldEqual(blankHbmIndexManyToMany.foreignkey);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var indexManyToManyMapping = new IndexManyToManyMapping();
            indexManyToManyMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "name1");
            var convertedHbmIndexManyToMany = converter.Convert(indexManyToManyMapping);
            convertedHbmIndexManyToMany.entityname.ShouldEqual(indexManyToManyMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var indexManyToManyMapping = new IndexManyToManyMapping();
            // Don't set anything on the original mapping
            var convertedHbmIndexManyToMany = converter.Convert(indexManyToManyMapping);
            var blankHbmIndexManyToMany = new HbmIndexManyToMany();
            convertedHbmIndexManyToMany.entityname.ShouldEqual(blankHbmIndexManyToMany.entityname);
        }

        [Test]
        public void ShouldConvertColumns()
        {
            ShouldConvertSubobjectsAsLooselyTypedArray<IndexManyToManyMapping, ColumnMapping, HbmIndexManyToMany, HbmColumn, object>(
                (indexManyToManyMapping, columnMapping) => indexManyToManyMapping.AddColumn(Layer.Defaults, columnMapping),
                hbmIndexManyToMany => hbmIndexManyToMany.column);
        }
    }
}