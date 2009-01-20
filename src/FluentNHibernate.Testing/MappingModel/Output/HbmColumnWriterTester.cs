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
            var testHelper = new HbmTestHelper<ColumnMapping, HbmColumn>();
            testHelper.Check(x => x.Name, "Column1").MapsTo(x => x.name);
            testHelper.Check(x => x.Length, 50).MapsTo(x => x.length, "50");
            testHelper.Check(x => x.AllowNull, false)
                .MapsTo(x => x.notnull, true)
                .MapsTo(x => x.notnullSpecified, true);
            testHelper.Check(x => x.Unique, true)
                .MapsTo(x => x.unique)
                .MapsTo(x => x.uniqueSpecified, true);
            testHelper.Check(x => x.UniqueKey, "key").MapsTo(x => x.uniquekey);
            testHelper.Check(x => x.SqlType, "nvarchar").MapsTo(x => x.sqltype);
            testHelper.Check(x => x.Index, "index1").MapsTo(x => x.index);
            testHelper.Check(x => x.Check, "checkSomething").MapsTo(x => x.check);

            var writer = new HbmColumnWriter();
            testHelper.VerifyAll(writer);
        }
    }
}