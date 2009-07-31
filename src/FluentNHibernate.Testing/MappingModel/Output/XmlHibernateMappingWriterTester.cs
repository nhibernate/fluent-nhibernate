using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlHibernateMappingWriterTester
    {
        private IXmlWriter<HibernateMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<HibernateMapping>>();
        }

        [Test]
        public void ShouldOnlyOutputOneClass()
        {
            var mapping = new HibernateMapping();

            mapping.AddClass(new ClassMapping());

            writer.VerifyXml(mapping)
                .Element("class[2]").DoesntExist();
        }

        [Test]
        public void ShouldWriteSchemaAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.Schema, "dbo").MapsToAttribute("schema");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDefaultCascadeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.DefaultCascade, "cas").MapsToAttribute("default-cascade");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDefaultAccessAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.DefaultAccess, "acc").MapsToAttribute("default-access");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAutoImportAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.AutoImport, true).MapsToAttribute("auto-import");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteDefaultLazyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.DefaultLazy, true).MapsToAttribute("default-lazy");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCatalogAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.Catalog, "catalog").MapsToAttribute("catalog");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNamespaceAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.Namespace, "namespace").MapsToAttribute("namespace");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteAssemblyAttribute()
        {
            var testHelper = new XmlWriterTestHelper<HibernateMapping>();

            testHelper.Check(x => x.Assembly, "assembly").MapsToAttribute("assembly");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteImports()
        {
            var mapping = new HibernateMapping();

            mapping.AddImport(new ImportMapping());

            writer.VerifyXml(mapping)
                .Element("import").Exists();
        }

        [Test]
        public void ShouldWriteClasses()
        {
            var mapping = new HibernateMapping();

            mapping.AddClass(new ClassMapping());

            writer.VerifyXml(mapping)
                .Element("class").Exists();
        }
    }
}