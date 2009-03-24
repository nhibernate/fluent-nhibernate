using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassCacheTests
    {
        [Test]
        public void should_be_first_element()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping =>
                {
                    mapping.Id(x => x.Id);
                    mapping.Map(x => x.Name);
                    mapping.Cache.AsReadWrite();
                })
                .Element("class/*[1]").HasName("cache");
        }

        [Test]
        public void should_create_cache_element()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsReadWrite())
                .Element("class/cache").Exists();
        }

        [Test]
        public void should_output_read_write_for_AsReadWrite()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsReadWrite())
                .Element("class/cache").HasAttribute("usage", "read-write");
        }

        [Test]
        public void should_output_nonstrict_read_write_for_AsNonStrictReadWrite()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsNonStrictReadWrite())
                .Element("class/cache").HasAttribute("usage", "nonstrict-read-write");
        }

        [Test]
        public void should_output_nonstrict_read_write_for_AsReadOnly()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsReadOnly())
                .Element("class/cache").HasAttribute("usage", "read-only");
        }

        [Test]
        public void should_allow_anything_for_AsCustom()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsCustom("something-else"))
                .Element("class/cache").HasAttribute("usage", "something-else");
        }

        [Test]
        public void ShouldWriteRegionWhenAssigned()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsReadWrite().Region("MyRegion"))
                .Element("class/cache").HasAttribute("region", "MyRegion");
        }

        [Test]
        public void ShouldWriteCustomAttributes()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(mapping => mapping.Cache.AsReadWrite().SetAttribute("custom","attribute"))
                .Element("class/cache").HasAttribute("custom", "attribute");
        }

        private class CacheTarget
        {
            public virtual int Id { get; set; }
            public virtual string Name { get; set; }
        }
    }
}