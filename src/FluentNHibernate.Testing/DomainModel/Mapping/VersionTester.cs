using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class VersionTester
    {
        private ClassMap<VersionTarget> _classMap;

        [SetUp]
        public void SetUp()
        {
            _classMap = new ClassMap<VersionTarget>();
        }

        [Test]
        public void CanSpecifyVersion()
        {
            var versionPart = _classMap.Version(p => p.VersionNumber);
            Assert.IsNotNull(versionPart);

            var document = _classMap.CreateMapping(new MappingVisitor());
            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//version");
            keyElement.AttributeShouldEqual("column", "VersionNumber");
        }

        [Test]
        public void CanSpecifyVersionOverrideColumnName()
        {
            var versionPart = _classMap
                                    .Version(p => p.VersionNumber)
                                    .TheColumnNameIs("Version");

            Assert.IsNotNull(versionPart);

            var document = _classMap.CreateMapping(new MappingVisitor());
            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//version");
            keyElement.AttributeShouldEqual("name", "VersionNumber");
            keyElement.AttributeShouldEqual("column", "Version");
        }

        [Test]
        public void CanSepecifyAccessType()
        {
            var versionPart = _classMap
                        .Version(p => p.VersionNumber)
                        .Access.AsField();

            Assert.IsNotNull(versionPart);

            var document = _classMap.CreateMapping(new MappingVisitor());
            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//version");
            keyElement.AttributeShouldEqual("name", "VersionNumber");
            keyElement.AttributeShouldEqual("column", "VersionNumber");
            keyElement.AttributeShouldEqual("access", "field");
        }

        [Test]
        public void CanSepecifyAsNeverGenerated()
        {
            var versionPart = _classMap
                        .Version(p => p.TimeStamp);

            Assert.IsNotNull(versionPart);

            var document = _classMap.CreateMapping(new MappingVisitor());
            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//version");
            keyElement.AttributeShouldEqual("type", "timestamp");
        }
    }

    public class VersionTarget
    {
        public int VersionNumber { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}