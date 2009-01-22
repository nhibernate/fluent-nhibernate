using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmManyToOneWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var manyToOne = new ManyToOneMapping { Name = "manyToOne" };
            var writer = new HbmManyToOneWriter();

            writer.ShouldGenerateValidOutput(manyToOne);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Name, "manyToOne").MapsToAttribute("name");
            testHelper.Check(x => x.IsNotNullable, true).MapsToAttribute("not-null");

            var writer = new HbmManyToOneWriter();
            testHelper.VerifyAll(writer);
        }
    }
}
