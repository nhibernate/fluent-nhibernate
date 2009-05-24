using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlGeneratorWriterTester
    {
        private XmlGeneratorWriter writer;

        [Test]
        public void ShouldWriteRegionAttribute()
        {
            writer = new XmlGeneratorWriter();
            var testHelper = new XmlWriterTestHelper<GeneratorMapping>();
            testHelper.Check(x => x.Class, "class").MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }
    }
}