using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlOneToOneWriterTester
    {
        private XmlOneToOneWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Cascade, "cascade").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Class, new TypeReference("type")).MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteConstrainedAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Constrained, true).MapsToAttribute("constrained");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Fetch, "always").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteForeignKeyAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.Name, "nm").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePropertyRefAttribute()
        {
            writer = new XmlOneToOneWriter();
            var testHelper = new XmlWriterTestHelper<OneToOneMapping>();
            testHelper.Check(x => x.PropertyRef, "pr").MapsToAttribute("property-ref");

            testHelper.VerifyAll(writer);
        }
    }
}