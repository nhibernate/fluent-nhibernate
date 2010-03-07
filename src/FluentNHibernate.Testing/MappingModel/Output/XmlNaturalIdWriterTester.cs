using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlNaturalIdWriterTester
    {
        private IXmlWriter<NaturalIdMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<NaturalIdMapping>>();
        }

        [Test]
        public void ShouldWriteMutableAttribute()
        {
            var testHelper = new XmlWriterTestHelper<NaturalIdMapping>();

            testHelper.Check(x => x.Mutable, true).MapsToAttribute("mutable");
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteProperties()
        {
            var mapping = new NaturalIdMapping();
            mapping.AddProperty(new PropertyMapping());

            writer.VerifyXml(mapping)
                .Element("property").Exists();
        }

        [Test]
        public void ShouldWriteManyToOnes()
        {
            var mapping = new NaturalIdMapping();
            mapping.AddReference(new ManyToOneMapping());

            writer.VerifyXml(mapping)
                .Element("many-to-one").Exists();
        }

    }
}
