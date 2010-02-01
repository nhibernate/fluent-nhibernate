using System.Linq;
using FluentNHibernate.Automapping;
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
                AutoMap.Source(new StubTypeSource(new[] { typeof(CacheTarget) }))
                    .Override<CacheTarget>(x => x.Cache.ReadOnly());

            var classMapping = automapper
                .BuildMappings()
                .SelectMany(x => x.Classes)
                .First();

            classMapping.Cache.Usage.ShouldEqual("read-only");
        }
    }

    class CacheTarget
    {
        public int Id { get; set; }
    }
}