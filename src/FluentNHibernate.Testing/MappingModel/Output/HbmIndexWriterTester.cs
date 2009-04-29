using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmIndexWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var index = new IndexMapping();
            var writer = new HbmIndexWriter();

            writer.ShouldGenerateValidOutput(index);                
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<IndexMapping>();
            testHelper.Check(x => x.Column, "column1").MapsToAttribute("column");
            testHelper.Check(x => x.IndexType, "String").MapsToAttribute("type");
            testHelper.Check(x => x.Length, 50).MapsToAttribute("length", "50");

            var writer = new HbmIndexWriter();
            testHelper.VerifyAll(writer);
        }
    }
}
