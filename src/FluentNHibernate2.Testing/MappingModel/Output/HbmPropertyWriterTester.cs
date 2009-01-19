using System.Reflection;
using NUnit.Framework;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmPropertyWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var property = new PropertyMapping {Name = "Property1"};
            var writer = new HbmPropertyWriter();
            writer.ShouldGenerateValidOutput(property);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<PropertyMapping, HbmProperty>();
            testHelper.Check(x => x.Length, 50).MapsTo(x => x.length, "50");
            testHelper.Check(x => x.AllowNull, false).MapsTo(x => x.notnull, true);
            testHelper.Check(x => x.Name, "test").MapsTo(x => x.name);

            var writer = new HbmPropertyWriter();
            testHelper.VerifyAll(writer);
        }

        
    }
}