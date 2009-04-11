using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmColumnWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var column = new ColumnMapping() {Name = "Column1"};
            var writer = new HbmColumnWriter();

            writer.ShouldGenerateValidOutput(column);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ColumnMapping>();
            testHelper.Check(x => x.Name, "Column1").MapsToAttribute("name");
            testHelper.Check(x => x.Length, 50).MapsToAttribute("length");
            testHelper.Check(x => x.IsNotNullable, true).MapsToAttribute("not-null");
            testHelper.Check(x => x.IsUnique, true).MapsToAttribute("unique");                
            testHelper.Check(x => x.UniqueKey, "key").MapsToAttribute("unique-key");
            testHelper.Check(x => x.SqlType, "nvarchar").MapsToAttribute("sql-type");
            testHelper.Check(x => x.Index, "index1").MapsToAttribute("index");
            testHelper.Check(x => x.Check, "checkSomething").MapsToAttribute("check");

            var writer = new HbmColumnWriter();
            testHelper.VerifyAll(writer);
        }
    }
}