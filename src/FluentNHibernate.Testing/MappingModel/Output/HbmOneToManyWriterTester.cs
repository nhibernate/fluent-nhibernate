using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmOneToManyWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var oneToMany = new OneToManyMapping() { ClassName = "class1" };
            var writer = new HbmOneToManyWriter();
            writer.ShouldGenerateValidOutput(oneToMany);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<OneToManyMapping, HbmOneToMany>();
            testHelper.Check(x => x.ClassName, "class1").MapsTo(x => x.@class);
            testHelper.Check(x => x.ExceptionOnNotFound, true)
                .MapsTo(x => x.notfound, HbmNotFoundMode.Exception)
                .MapsTo(x => x.notfoundSpecified, true);
            testHelper.Check(x => x.ExceptionOnNotFound, false)
                .MapsTo(x => x.notfound, HbmNotFoundMode.Ignore)
                .MapsTo(x => x.notfoundSpecified, true);

            var writer = new HbmOneToManyWriter();
            testHelper.VerifyAll(writer);
        }


    }
}