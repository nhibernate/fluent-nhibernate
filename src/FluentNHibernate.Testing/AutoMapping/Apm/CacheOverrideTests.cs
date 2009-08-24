using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public class CacheOverrideTests
    {
        [Test]
        public void ShouldBeAbleToSpecifyCacheInOverride()
        {
            var automapper =
                AutoMap.Source(new StubTypeSource(new[] { typeof(Target) }))
                    .Override<Target>(x => x.Cache.ReadOnly());

            automapper.CompileMappings();
            var classMapping = automapper
                .BuildMappings()
                .SelectMany(x => x.Classes)
                .First();

            classMapping.Cache.Usage.ShouldEqual("read-only");
        }
    }
}