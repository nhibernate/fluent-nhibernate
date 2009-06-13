using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlOneToManyWriterTester
    {
        private XmlOneToManyWriter writer;

        [Test]
        public void ShouldWriteClassAttribute()
        {
            writer = new XmlOneToManyWriter();
            var testHelper = new XmlWriterTestHelper<OneToManyMapping>();
            testHelper.Check(x => x.Class, new TypeReference("type")).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotFoundAttribute()
        {
            writer = new XmlOneToManyWriter();
            var testHelper = new XmlWriterTestHelper<OneToManyMapping>();
            testHelper.Check(x => x.NotFound, "nf").MapsToAttribute("not-found");

            testHelper.VerifyAll(writer);
        }
    }
}