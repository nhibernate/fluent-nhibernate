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
            var testHelper = new HbmTestHelper<PropertyMapping>();
            testHelper.Check(x => x.Name, "test").MapsToAttribute("name");
            testHelper.Check(x => x.Length, 50).MapsToAttribute("length");
            testHelper.Check(x => x.IsNotNullable, true).MapsToAttribute("not-null");
            testHelper.Check(x => x.ColumnName, "thecolumn").MapsToAttribute("column");
            testHelper.Check(x => x.Unique, true).MapsToAttribute("unique");
            var writer = new HbmPropertyWriter();
            testHelper.VerifyAll(writer);
        }

        
    }
}