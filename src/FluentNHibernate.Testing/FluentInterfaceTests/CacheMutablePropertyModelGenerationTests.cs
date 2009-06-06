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
        public void UsageShouldSetModelUsagePropertyToValue()
        {
            Cache()
                .Mapping(m => m.AsReadOnly())
                .ModelShouldMatch(x => x.Usage.ShouldEqual("read-only"));
        }
    }
}