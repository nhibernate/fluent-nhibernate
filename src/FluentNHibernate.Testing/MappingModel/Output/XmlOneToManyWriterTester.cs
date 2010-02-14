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

        [SetUp]
        public void SetUp()
        {
            writer = new XmlOneToManyWriter();
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            var testHelper = new XmlWriterTestHelper<OneToManyMapping>();
            testHelper.Check(x => x.Class, new TypeReference("type")).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotFoundAttribute()
        {
            var testHelper = new XmlWriterTestHelper<OneToManyMapping>();
            testHelper.Check(x => x.NotFound, "nf").MapsToAttribute("not-found");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteEntityName()
        {
            var testHelper = new XmlWriterTestHelper<OneToManyMapping>();
            testHelper.Check(x => x.EntityName, "name1").MapsToAttribute("entity-name");

            testHelper.VerifyAll(writer);
        }
    }
}