using NUnit.Framework;
using ShadeTree.DomainModel.Query;

namespace ShadeTree.Testing.DomainModel.Query
{
    [TestFixture]
    public class FilterTypeRegistryTester
    {
        [SetUp]
        public void SetUp()
        {
            FilterTypeRegistry.ResetAll();
        }

        [Test]
        public void should_return_expected_filter_types_for_string_type()
        {
            var filters = FilterTypeRegistry.GetFiltersFor<string>();
            filters.ShouldContain(x => x.Key == "STARTSWITH");
        }

        [Test]
        public void should_return_expected_filter_types_for_int32_type()
        {
            var filters = FilterTypeRegistry.GetFiltersFor<int>();
            filters.ShouldContain(x => x.Key == "EQUAL");
        }
    }
}