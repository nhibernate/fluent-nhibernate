using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.TestFixtures;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping;

[TestFixture]
public class VersionTests : BaseAutoPersistenceTests
{
    AutoMappingTester<TEntity> VerifyAutoMap<TEntity>()
    {
        var autoMapper = AutoMap.Source(new StubTypeSource(typeof(TEntity)));

        return new AutoMappingTester<TEntity>(autoMapper);
    }

    [Test]
    public void PropertyNamedTimestampMappedAsVersion()
    {
        VerifyAutoMap<ValidTimestampClass>()
            .Element("//version").HasAttribute("name", "Timestamp")
            .Element("//version/column").HasAttribute("name", "Timestamp");
    }

    [Test]
    public void PropertyNamedVersionMappedAsVersion()
    {
        VerifyAutoMap<ValidVersionClass>()
            .Element("//version").HasAttribute("name", "Version")
            .Element("//version/column").HasAttribute("name", "Version");
    }

    [Test]
    public void PropertyNamedTimestampWithInvalidTypeNotMappedAsVersion()
    {
        VerifyAutoMap<InvalidTimestampClass>()
            .Element("//version")
            .DoesntExist();
    }

    [Test]
    public void PropertyNamedVersionWithInvalidTypeNotMappedAsVersion()
    {
        VerifyAutoMap<InvalidVersionClass>()
            .Element("//version")
            .DoesntExist();
    }
}
