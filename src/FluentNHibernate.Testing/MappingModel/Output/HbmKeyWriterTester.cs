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
            var testHelper = new HbmTestHelper<KeyMapping, HbmKey>();
            testHelper.Check(x => x.Column, "column1").MapsTo(x => x.column1);
            testHelper.Check(x => x.ForeignKey, "foreignKey").MapsTo(x => x.foreignkey);
            testHelper.Check(x => x.PropertyReference, "SomeProperty1").MapsTo(x => x.propertyref);
            testHelper.Check(x => x.CascadeOnDelete, true)
                .MapsTo(x => x.ondelete, HbmOndelete.Cascade)
                .MapsTo(x => x.ondeleteSpecified, true);
            testHelper.Check(x => x.CascadeOnDelete, false)
                .MapsTo(x => x.ondelete, HbmOndelete.Noaction)
                .MapsTo(x => x.ondeleteSpecified, true);

            var writer = new HbmKeyWriter();
            testHelper.VerifyAll(writer);
            

        }
    }
}