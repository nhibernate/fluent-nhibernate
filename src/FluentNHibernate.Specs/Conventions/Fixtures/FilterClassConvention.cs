using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class FilterClassConvention : IClassConvention
    {
        public const string FilterName = "TestFilterName";
        public const string FilterCondition = "TestFilterCondition";

        public void Apply(IClassInstance instance)
        {
            instance.ApplyFilter(FilterName, FilterCondition);
        }
    }

    public class FilterHasManyConvention : IHasManyConvention
    {
        public const string FilterName = "TestFilterName";
        public const string FilterCondition = "TestFilterCondition";

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.ApplyFilter(FilterName, FilterCondition);
        }
    }

    public class FilterHasManyToManyConvention : IHasManyToManyConvention
    {
        public const string FilterName = "TestFilterName";
        public const string FilterCondition = "TestFilterCondition";

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.ApplyFilter(FilterName, FilterCondition);
        }
    }
}