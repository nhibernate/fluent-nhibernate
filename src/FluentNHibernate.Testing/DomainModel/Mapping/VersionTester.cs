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
            new MappingTester<VersionTarget>()
                .ForMapping(map => map.Version(x => x.VersionNumber))
                .Element("//version")
                    .Exists()
                    .HasAttribute("column", "VersionNumber");
        }

        [Test]
        public void CanSpecifyVersionOverrideColumnName()
        {
            new MappingTester<VersionTarget>()
                .ForMapping(map => map.Version(x => x.VersionNumber).ColumnName("Version"))
                .Element("//version").HasAttribute("column", "Version");
        }

        [Test]
        public void CanSepecifyAccessType()
        {
            new MappingTester<VersionTarget>()
                .ForMapping(map => map.Version(x => x.VersionNumber).Access.AsField())
                .Element("//version").HasAttribute("access", "field");
        }

        [Test]
        public void CanSpecifyTimestamp()
        {
            new MappingTester<VersionTarget>()
                .ForMapping(map => map.Version(x => x.TimeStamp))
                .Element("//version").HasAttribute("type", "timestamp");
        }

        [Test]
        public void CanSepecifyAsNeverGenerated()
        {
            new MappingTester<VersionTarget>()
                .ForMapping(map => map.Version(x => x.VersionNumber).NeverGenerated())
                .Element("//version").HasAttribute("generated", "never");
        }
    }

    public class VersionTarget
    {
        public int VersionNumber { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}