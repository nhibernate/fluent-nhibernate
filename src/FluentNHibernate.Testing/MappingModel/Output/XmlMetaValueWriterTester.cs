using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlMetaValueWriterTester
    {
        private XmlMetaValueWriter writer;

        [Test]
        public void ShouldWriteValueAttribute()
        {
            writer = new XmlMetaValueWriter();
            var testHelper = new XmlWriterTestHelper<MetaValueMapping>();
            testHelper.Check(x => x.Value, "val").MapsToAttribute("value");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            writer = new XmlMetaValueWriter();
            var testHelper = new XmlWriterTestHelper<MetaValueMapping>();
            testHelper.Check(x => x.Class, new TypeReference("type")).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }
    }
}