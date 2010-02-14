using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlStoredProcedureWriterTester
    {
        private IXmlWriter<ClassMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ClassMapping>>();
        }

        [Test]
        public void ShouldWriteSqlUpdate()
        {
            var mapping = new ClassMapping();

            mapping.AddStoredProcedure(new StoredProcedureMapping("sql-update", "update ABC"));

            writer.VerifyXml(mapping)
                .Element("sql-update").Exists();
        }

        [Test]
        public void ShouldWriteCheckAttribute()
        {
            IXmlWriter<StoredProcedureMapping> writer;
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<StoredProcedureMapping>>();

            var testHelper = new XmlWriterTestHelper<StoredProcedureMapping>();
            testHelper.Check(x => x.Check, "none").MapsToAttribute("check");

            testHelper.VerifyAll(writer);
        }      
 
    }
}
