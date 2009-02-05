using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmDiscriminatorWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var discriminator = new DiscriminatorMapping();

            var writer = new HbmDiscriminatorWriter();
            writer.ShouldGenerateValidOutput(discriminator);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<DiscriminatorMapping>();
            testHelper.Check(x => x.Column, "col1").MapsToAttribute("column");
            testHelper.Check(x => x.DiscriminatorType, DiscriminatorType.Char).MapsToAttribute("type", DiscriminatorType.Char.ToString());
            testHelper.Check(x => x.Force, true).MapsToAttribute("force");
            testHelper.Check(x => x.Formula, "some formula").MapsToAttribute("formula");
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");
            testHelper.Check(x => x.IsNotNullable, false).MapsToAttribute("not-null");
            testHelper.Check(x => x.Length, 50).MapsToAttribute("length");

            var writer = new HbmDiscriminatorWriter();
            testHelper.VerifyAll(writer);
        }
    }
}
