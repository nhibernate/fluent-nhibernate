using System.Linq;
using FluentNHibernate.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm;

[TestFixture]
public class IgnoreBaseAutoPersistenceModelTests
{
    [Test]
    public void ShouldntMapTypesIgnoredByIgnoreBase()
    {
        var automapper =
            AutoMap.Source(new StubTypeSource(new[] {typeof(Entity), typeof(RealBase)}))
                .IgnoreBase<Entity>();

        var mappings = automapper.BuildMappings();

        mappings.SelectMany(x => x.Classes)
            .ShouldNotContain(x => x.Type == typeof(Entity), "Entity shouldn't be mapped when ignored by IgnoreBase");
    }

    [Test]
    public void ShouldntMapAnyTypesIgnoredByIgnoreBase()
    {
        var automapper =
            AutoMap.Source(new StubTypeSource(new[] { typeof(Entity), typeof(RealBase) }))
                .IgnoreBase<Entity>()
                .IgnoreBase<RealBase>();

        var mappings = automapper.BuildMappings()
            .SelectMany(x => x.Classes);

        mappings.ShouldNotContain(x => x.Type == typeof(Entity), "Entity shouldn't be mapped when ignored by IgnoreBase");
        mappings.ShouldNotContain(x => x.Type == typeof(RealBase), "RealBase shouldn't be mapped when ignored by IgnoreBase");
    }
}

class Entity
{
    public int Id { get; set; }
}

class RealBase : Entity
{}
