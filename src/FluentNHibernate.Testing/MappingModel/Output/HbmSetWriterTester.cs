using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using FluentNHibernate.MappingModel;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmSetWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var set = new SetMapping { Name = "set1", Contents = new OneToManyMapping(), Key = new KeyMapping() };

            var contentsWriter = MockRepository.GenerateStub<IHbmWriter<ICollectionContentsMapping>>();
            contentsWriter.Expect(x => x.Write(set.Contents)).Return(new HbmOneToMany { @class = "class1" });
            var keyWriter = MockRepository.GenerateStub<IHbmWriter<KeyMapping>>();
            keyWriter.Expect(x => x.Write(set.Key)).Return(new HbmKey());

            var writer = new HbmSetWriter(contentsWriter, keyWriter);

            writer.ShouldGenerateValidOutput(set);
        }

        [Test]
        public void Should_write_the_attributes()
        {

            var testHelper = new HbmTestHelper<SetMapping, HbmSet>();
            testHelper.Check(x => x.Name, "set1").MapsTo(x => x.name);
            testHelper.Check(x => x.OrderBy, "column1").MapsTo(x => x.orderby);
            testHelper.Check(x => x.IsInverse, true).MapsTo(x => x.inverse);
            testHelper.Check(x => x.IsLazy, true)
                .MapsTo(x => x.lazy)
                .MapsTo(x => x.lazySpecified);

            var writer = new HbmSetWriter(null, null);
            testHelper.VerifyAll(writer);
        }
    }
}
