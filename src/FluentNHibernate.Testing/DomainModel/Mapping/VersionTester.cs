using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class VersionTester
{
    ClassMap<VersionTarget> _classMap;

    [SetUp]
    public void SetUp()
    {
        _classMap = new ClassMap<VersionTarget>();
    }

    [Test]
    public void CanSpecifyVersion()
    {
        new MappingTester<VersionTarget>()
            .ForMapping(m =>
            {
                m.Id(x => x.Id);
                m.Version(x => x.VersionNumber);
            })
            .Element("//version").Exists()
            .Element("//version/column").HasAttribute("name", "VersionNumber");
    }

    [Test]
    public void CanSpecifyVersionOverrideColumnName()
    {
        new MappingTester<VersionTarget>()
            .ForMapping(m =>
            {
                m.Id(x => x.Id);
                m.Version(x => x.VersionNumber).Column("Version");
            })
            .Element("//version/column").HasAttribute("name", "Version");
    }

    [Test]
    public void CanSepecifyAccessType()
    {
        new MappingTester<VersionTarget>()
            .ForMapping(m =>
            {
                m.Id(x => x.Id);
                m.Version(x => x.VersionNumber).Access.Field();
            })
            .Element("//version").HasAttribute("access", "field");
    }

    [Test]
    public void CanSpecifyTimestamp()
    {
        new MappingTester<VersionTarget>()
            .ForMapping(m =>
            {
                m.Id(x => x.Id);
                m.Version(x => x.TimeStamp);
            })
            .Element("//version").HasAttribute("type", "timestamp");
    }

    [Test]
    public void CanSepecifyAsNeverGenerated()
    {
        new MappingTester<VersionTarget>()
            .ForMapping(m =>
            {
                m.Id(x => x.Id);
                m.Version(x => x.VersionNumber).Generated.Never();
            })
            .Element("//version").HasAttribute("generated", "never");
    }

    [Test]
    public void CanSepecifyUnsavedValue()
    {
        new MappingTester<VersionTarget>()
            .ForMapping(m =>
            {
                m.Id(x => x.Id);
                m.Version(x => x.VersionNumber).UnsavedValue("1");
            })
            .Element("//version").HasAttribute("unsaved-value", "1");
    }
}

public class VersionTarget
{
    public int Id { get; set; }
    public int VersionNumber { get; set; }
    public DateTime TimeStamp { get; set; }
}
