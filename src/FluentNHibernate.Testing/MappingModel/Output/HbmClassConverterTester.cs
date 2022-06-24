using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmClassConverterTester
    {
        private IHbmConverter<ClassMapping, HbmClass> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ClassMapping, HbmClass>>();
        }

        #region Value field tests

        [Test]
        public void ShouldConvertTableNameIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.TableName, Layer.Conventions, "tbl");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.table.ShouldEqual(classMapping.TableName);
        }

        [Test]
        public void ShouldNotConvertTableNameIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.table.ShouldEqual(blankHbmClass.table);
        }

        [Test]
        public void ShouldConvertSchemaIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Schema, Layer.Conventions, "dbo");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.schema.ShouldEqual(classMapping.Schema);
        }

        [Test]
        public void ShouldNotConvertSchemaIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.schema.ShouldEqual(blankHbmClass.schema);
        }

        [Test]
        public void ShouldConvertDiscriminatorValueIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.DiscriminatorValue, Layer.Conventions, 0);
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.discriminatorvalue.ShouldEqual(classMapping.DiscriminatorValue.ToString());
        }

        [Test]
        public void ShouldNotConvertDiscriminatorValueIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.discriminatorvalue.ShouldEqual(blankHbmClass.discriminatorvalue);
        }

        [Test]
        public void ShouldConvertMutableIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Mutable, Layer.Conventions, false); // Defaults to true, so we have to set it false here in order to tell if it actually changed
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.mutable.ShouldEqual(classMapping.Mutable);
        }

        [Test]
        public void ShouldNotConvertMutableIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.mutable.ShouldEqual(blankHbmClass.mutable);
        }

        [Test]
        public void ShouldConvertPolymorphismIfPopulated()
        {
            var polymorphism = HbmPolymorphismType.Explicit; // Defaults to Implicit, so use something else to properly detect that it changes

            var classMapping = new ClassMapping();
            var polyDict = new XmlLinkedEnumBiDictionary<HbmPolymorphismType>();
            classMapping.Set(fluent => fluent.Polymorphism, Layer.Conventions, polyDict[polymorphism]);
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.polymorphism.ShouldEqual(polymorphism);
        }

        [Test]
        public void ShouldNotConvertPolymorphismIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.polymorphism.ShouldEqual(blankHbmClass.polymorphism);
        }

        [Test]
        public void ShouldConvertPersisterIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Persister, Layer.Conventions, "p");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.persister.ShouldEqual(classMapping.Persister);
        }

        [Test]
        public void ShouldNotConvertPersisterIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.persister.ShouldEqual(blankHbmClass.persister);
        }

        [Test]
        public void ShouldConvertWhereIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Where, Layer.Conventions, "x = 1");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.where.ShouldEqual(classMapping.Where);
        }

        [Test]
        public void ShouldNotConvertWhereIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.where.ShouldEqual(blankHbmClass.where);
        }

        [Test]
        public void ShouldConvertBatchSizeIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.BatchSize, Layer.Conventions, 10);
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.batchsize.ShouldEqual(classMapping.BatchSize);
        }

        [Test]
        public void ShouldNotConvertBatchSizeIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.batchsize.ShouldEqual(blankHbmClass.batchsize);
        }

        [Test]
        public void ShouldConvertOptimisticLockIfPopulated()
        {
            var optimisticlock = HbmOptimisticLockMode.Dirty; // Defaults to Version, so use something else to properly detect that it changes

            var classMapping = new ClassMapping();
            var polyDict = new XmlLinkedEnumBiDictionary<HbmOptimisticLockMode>();
            classMapping.Set(fluent => fluent.OptimisticLock, Layer.Conventions, polyDict[optimisticlock]);
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.optimisticlock.ShouldEqual(optimisticlock);
        }

        [Test]
        public void ShouldNotConvertOptimisticLockIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.optimisticlock.ShouldEqual(blankHbmClass.optimisticlock);
        }

        [Test]
        public void ShouldConvertCheckIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Check, Layer.Conventions, "chk");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.check.ShouldEqual(classMapping.Check);
        }

        [Test]
        public void ShouldNotConvertCheckIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.check.ShouldEqual(blankHbmClass.check);
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Name, Layer.Conventions, "name");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.name.ShouldEqual(classMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.name.ShouldEqual(blankHbmClass.name);
        }

        [Test]
        public void ShouldConvertProxyIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Proxy, Layer.Conventions, "p");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.proxy.ShouldEqual(classMapping.Proxy);
        }

        [Test]
        public void ShouldNotConvertProxyIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.proxy.ShouldEqual(blankHbmClass.proxy);
        }

        [Test]
        public void ShouldConvertLazyIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Lazy, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.lazy.ShouldEqual(classMapping.Lazy);
        }

        [Test]
        public void ShouldNotConvertLazyIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.lazy.ShouldEqual(blankHbmClass.lazy);
        }

        [Test]
        public void ShouldConvertDynamicUpdateIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.DynamicUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.dynamicupdate.ShouldEqual(classMapping.DynamicUpdate);
        }

        [Test]
        public void ShouldNotConvertDynamicUpdateIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.dynamicupdate.ShouldEqual(blankHbmClass.dynamicupdate);
        }

        [Test]
        public void ShouldConvertDynamicInsertIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.DynamicInsert, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.dynamicinsert.ShouldEqual(classMapping.DynamicInsert);
        }

        [Test]
        public void ShouldNotConvertDynamicInsertIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.dynamicinsert.ShouldEqual(blankHbmClass.dynamicinsert);
        }

        [Test]
        public void ShouldConvertSelectBeforeUpdateIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.SelectBeforeUpdate, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.selectbeforeupdate.ShouldEqual(classMapping.SelectBeforeUpdate);
        }

        [Test]
        public void ShouldNotConvertSelectBeforeUpdateIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.selectbeforeupdate.ShouldEqual(blankHbmClass.selectbeforeupdate);
        }

        [Test]
        public void ShouldConvertAbstractIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Abstract, Layer.Conventions, true); // Defaults to false, so we have to set it true here in order to tell if it actually changed
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.@abstract.ShouldEqual(classMapping.Abstract);
        }

        [Test]
        public void ShouldNotConvertAbstractIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.@abstract.ShouldEqual(blankHbmClass.@abstract);
        }

        [Test]
        public void ShouldConvertSchemaActionIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.SchemaAction, Layer.Conventions, "none");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.schemaaction.ShouldEqual(classMapping.SchemaAction);
        }

        [Test]
        public void ShouldNotConvertSchemaActionIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.schemaaction.ShouldEqual(blankHbmClass.schemaaction);
        }

        [Test]
        public void ShouldConvertEntityNameIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.EntityName, Layer.Conventions, "entity1");
            var convertedHbmClass = converter.Convert(classMapping);
            convertedHbmClass.entityname.ShouldEqual(classMapping.EntityName);
        }

        [Test]
        public void ShouldNotConvertEntityNameIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.entityname.ShouldEqual(blankHbmClass.entityname);
        }

        #endregion Value field tests

        #region Non-converter-based subobject tests

        [Test]
        public void ShouldConvertHbmSubselect()
        {
            var subselect = new string[] { "val" };
            var convertedSubselect = HbmClassConverter.ToHbmSubselect(subselect);
            convertedSubselect.ShouldNotBeNull();
            convertedSubselect.Text.ShouldEqual(subselect);
        }

        [Test]
        public void ShouldConvertSubselectIfPopulated()
        {
            var classMapping = new ClassMapping();
            classMapping.Set(fluent => fluent.Subselect, Layer.Conventions, "val");
            var convertedHbmClass = converter.Convert(classMapping);

            // Since we check the actual conversion to an HbmSubselect value elsewhere, the only thing we can usefully check here
            // is that the field got populated.
            convertedHbmClass.subselect.ShouldNotBeNull();
        }

        [Test]
        public void ShouldNotConvertSubselectIfNotPopulated()
        {
            var classMapping = new ClassMapping();
            // Don't set the schema on the original mapping
            var convertedHbmClass = converter.Convert(classMapping);
            var blankHbmClass = new HbmClass();
            convertedHbmClass.subselect.ShouldEqual(blankHbmClass.subselect);
        }

        #endregion Non-converter-based subobject tests
    }
}
