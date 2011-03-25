using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class CollectionConvention : ICollectionConvention
    {
        public const string FilterName = "TestFilterName";
        public const string FilterCondition = "TestFilterCondition";

        public void Apply(ICollectionInstance instance)
        {
            instance.AsList();
        }
    }
}