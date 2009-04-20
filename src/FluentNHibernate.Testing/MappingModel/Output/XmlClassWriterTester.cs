using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.MappingModel.Identity;
using Rhino.Mocks;
using FluentNHibernate.MappingModel.Collections;
using StructureMap.AutoMocking;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlClassWriterTester
    {
        private RhinoAutoMocker<XmlClassWriter> _mocker;
        private XmlClassWriter _classWriter;

        [SetUp]
        public void SetUp()
        {
            _mocker = new RhinoAutoMocker<XmlClassWriter>();
            _classWriter = _mocker.ClassUnderTest;
        }

        [Test]
        public void Should_produce_valid_hbm()
        {
            var classMapping = new ClassMapping {Name = "class1", Id = new IdMapping()};

            _classWriter.Write(classMapping)
                .ShouldBeValidXml();
        }
    }
}