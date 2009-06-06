using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlCacheWriterTester
    {
        private XmlCacheWriter writer;

        [Test]
        public void ShouldWriteRegionAttribute()
        {
            writer = new XmlCacheWriter();
            var testHelper = new XmlWriterTestHelper<CacheMapping>();
            testHelper.Check(x => x.Region, "region").MapsToAttribute("region");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUsageAttribute()
        {
            writer = new XmlCacheWriter();
            var testHelper = new XmlWriterTestHelper<CacheMapping>();
            testHelper.Check(x => x.Usage, "usage").MapsToAttribute("usage");

            testHelper.VerifyAll(writer);
        }
    }
}