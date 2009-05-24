using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlManyToOneWriterTester
    {
        private XmlManyToOneWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }
    }
}