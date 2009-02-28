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
    public class HbmManyToManyWriterTester
    {
        [Test]
        public void Should_generate_valid_hbm()
        {
            var manyToMany = new ManyToManyMapping {ClassName = "Class1"};
            var writer = new HbmManyToManyWriter();

            writer.ShouldGenerateValidOutput(manyToMany);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ManyToManyMapping>();
            testHelper.Check(x => x.ClassName, "class1").MapsToAttribute("class");

            var writer = new HbmManyToManyWriter();
            testHelper.VerifyAll(writer);
        }

    }
}
