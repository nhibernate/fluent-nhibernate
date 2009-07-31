using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class CacheMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void RegionShouldSetModelAccessRegionToValue()
        {
            Cache()
                .Mapping(m => m.Region("region"))
                .ModelShouldMatch(x => x.Region.ShouldEqual("region"));
        }

        [Test]
        public void ReadOnlyShouldSetModelUsageToReadOnly()
        {
            Cache()
                .Mapping(m => m.ReadOnly())
                .ModelShouldMatch(x => x.Usage.ShouldEqual("read-only"));
        }

        [Test]
        public void ReadWriteShouldSetModelUsageToReadWrite()
        {
            Cache()
                .Mapping(m => m.ReadWrite())
                .ModelShouldMatch(x => x.Usage.ShouldEqual("read-write"));
        }

        [Test]
        public void NonStrictReadWriteShouldSetModelUsageToNonStrictReadWrite()
        {
            Cache()
                .Mapping(m => m.NonStrictReadWrite())
                .ModelShouldMatch(x => x.Usage.ShouldEqual("nonstrict-read-write"));
        }

        [Test]
        public void CustomUsageShouldSetModelUsageToValue()
        {
            Cache()
                .Mapping(m => m.CustomUsage("usage"))
                .ModelShouldMatch(x => x.Usage.ShouldEqual("usage"));
        }

        [Test]
        public void IncludeAllShouldSetModelIncludeToAll()
        {
            Cache()
                .Mapping(m => m.IncludeAll())
                .ModelShouldMatch(x => x.Include.ShouldEqual("all"));
        }

        [Test]
        public void IncludeNonLazyShouldSetModelIncludeToNonLazy()
        {
            Cache()
                .Mapping(m => m.IncludeNonLazy())
                .ModelShouldMatch(x => x.Include.ShouldEqual("non-lazy"));
        }

        [Test]
        public void CustomIncludeShouldSetModelIncludeToValue()
        {
            Cache()
                .Mapping(m => m.CustomInclude("include"))
                .ModelShouldMatch(x => x.Include.ShouldEqual("include"));
        }
    }
}