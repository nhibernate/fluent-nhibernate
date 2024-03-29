using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions;

[TestFixture]
public class VersionConventionTests
{
    [Test]
    public void ShouldBeAbleToSpecifyColumnInConvention()
    {
        var model =
            AutoMap.Source(new StubTypeSource(typeof(VersionTarget)))
                .Conventions.Add<VersionConvention>();

        var classMapping = model.BuildMappings()
            .First()
            .Classes.First();

        classMapping.Version.Columns.First().Name.ShouldEqual("xxx");
    }

    class VersionConvention : IVersionConvention
    {
        public void Apply(IVersionInstance instance)
        {
            instance.Column("xxx");
        }
    }
}

class VersionTarget
{
    public int Id { get; set; }
    public byte[] Version { get; set; }
}
