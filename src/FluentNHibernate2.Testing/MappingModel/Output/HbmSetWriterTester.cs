using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmSetWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var bag = new SetMapping
            {
                Name = "set1",
                Key = new KeyMapping(),
                Contents = new OneToManyMapping { ClassName = "class1" }
            };
            var writer = new HbmSetWriter(HbmMother.CreateCollectionContentsWriter(), HbmMother.CreateKeyWriter());

            writer.ShouldGenerateValidOutput(bag);
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
