using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlVersionWriterTester
    {
        private XmlVersionWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlVersionWriter();
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteColumnAttribute()
        {
            writer = new XmlVersionWriter();
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Column, "col").MapsToAttribute("column");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteGeneratedAttribute()
        {
            writer = new XmlVersionWriter();
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Generated, "always").MapsToAttribute("generated");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlVersionWriter();
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Name, "name").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeAttribute()
        {
            writer = new XmlVersionWriter();
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.Type, new TypeReference("type")).MapsToAttribute("type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUnsavedValueAttribute()
        {
            writer = new XmlVersionWriter();
            var testHelper = new XmlWriterTestHelper<VersionMapping>();
            testHelper.Check(x => x.UnsavedValue, "u-value").MapsToAttribute("unsaved-value");

            testHelper.VerifyAll(writer);
        }
    }
}