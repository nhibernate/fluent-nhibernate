using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmBagWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var bag = new BagMapping { Name = "bag1", Contents = new OneToManyMapping(), Key = new KeyMapping() };

            var contentsWriter = MockRepository.GenerateStub<IHbmWriter<ICollectionContentsMapping>>();
            contentsWriter.Expect(x => x.Write(bag.Contents)).Return(new HbmOneToMany { @class = "class1" });
            var keyWriter = MockRepository.GenerateStub<IHbmWriter<KeyMapping>>();
            keyWriter.Expect(x => x.Write(bag.Key)).Return(new HbmKey());

            var writer = new HbmBagWriter(contentsWriter, keyWriter);

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