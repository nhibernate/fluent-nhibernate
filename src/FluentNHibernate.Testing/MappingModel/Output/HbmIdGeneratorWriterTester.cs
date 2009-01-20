using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIdGeneratorWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var generator = new IdGeneratorMapping() { ClassName = "native"};
            var writer = new HbmIdGeneratorWriter();
            writer.ShouldGenerateValidOutput(generator);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<IdGeneratorMapping, HbmGenerator>();
            testHelper.Check(x => x.ClassName, "native").MapsTo(x => x.@class);

            var writer = new HbmIdGeneratorWriter();
            testHelper.VerifyAll(writer);
        }
    }
}