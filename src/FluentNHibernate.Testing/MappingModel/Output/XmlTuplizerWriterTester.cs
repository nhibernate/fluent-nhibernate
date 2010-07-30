using System;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlTuplizerWriterTester
    {
        private IXmlWriter<TuplizerMapping> writer;

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<TuplizerMapping>>();
        }

        [Test]
        public void ShouldWriteModeAttribute()
        {
            var testHelper = new XmlWriterTestHelper<TuplizerMapping>();
            testHelper.Check(x => x.Mode, TuplizerMode.DynamicMap).MapsToAttribute("entity-mode", "dynamic-map");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteEntityNameAttribute()
        {
            var testHelper = new XmlWriterTestHelper<TuplizerMapping>();
            testHelper.Check(x => x.EntityName, "test").MapsToAttribute("entity-name", "test");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            Type tuplizerType = typeof(NHibernate.Tuple.Entity.PocoEntityTuplizer);

            var testHelper = new XmlWriterTestHelper<TuplizerMapping>();
            testHelper.Check(x => x.Type, new TypeReference(tuplizerType)).MapsToAttribute("class", tuplizerType.AssemblyQualifiedName);

            testHelper.VerifyAll(writer);
        }
    }
}