using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions
{
    [TestFixture]
    public class VersionConventionTests
    {
        [Test]
        public void ShouldBeAbleToSpecifyColumnInConvention()
        {
            var model =
                AutoMap.Source(new StubTypeSource(typeof(VersionTarget)))
                    .Conventions.Add<VersionConvention>();

            model.CompileMappings();

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Version.Columns.First().Name.ShouldEqual("xxx");
        }

        private class VersionConvention : IVersionConvention
        {
            public void Apply(IVersionInstance instance)
            {
                instance.Column("xxx");
            }
        }
    }

    internal class VersionTarget
    {
        public byte[] Version { get; set; }
    }
}