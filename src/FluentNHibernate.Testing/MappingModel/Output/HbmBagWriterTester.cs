using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmBagWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var bag = new BagMapping
            {
                Name = "bag1",
                Key = new KeyMapping(),
                Contents = new OneToManyMapping { ClassName = "class1" }
            };
            var writer = new HbmBagWriter(HbmMother.CreateCollectionContentsWriter(), HbmMother.CreateKeyWriter());

            writer.ShouldGenerateValidOutput(bag);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<BagMapping, HbmBag>();
            testHelper.Check(x => x.Name, "bag1").MapsTo(x => x.name);
            testHelper.Check(x => x.OrderBy, "column1").MapsTo(x => x.orderby);
            testHelper.Check(x => x.IsInverse, true).MapsTo(x => x.inverse);
            testHelper.Check(x => x.IsLazy, true)
                .MapsTo(x => x.lazy)
                .MapsTo(x => x.lazySpecified);

            var writer = new HbmBagWriter(null, null);
            testHelper.VerifyAll(writer);
        }

    }
}