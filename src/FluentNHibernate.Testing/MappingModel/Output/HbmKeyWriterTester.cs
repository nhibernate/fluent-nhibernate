using FluentNHibernate.MappingModel;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmKeyWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var key = new KeyMapping();
            var writer = new HbmKeyWriter();

            writer.ShouldGenerateValidOutput(key);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<KeyMapping>();
            testHelper.Check(x => x.Column, "column1").MapsToAttribute("column");
            testHelper.Check(x => x.ForeignKey, "foreignKey").MapsToAttribute("foreign-key");
            testHelper.Check(x => x.PropertyReference, "SomeProperty1").MapsToAttribute("property-ref");
            testHelper.Check(x => x.CascadeOnDelete, true).MapsToAttribute("on-delete", HbmOndelete.Cascade);

            var writer = new HbmKeyWriter();
            testHelper.VerifyAll(writer);
            

        }
    }
}