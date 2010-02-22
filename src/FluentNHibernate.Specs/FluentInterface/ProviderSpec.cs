using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Specs.FluentInterface
{
    public abstract class ProviderSpec
    {
        public static ClassMapping map_as_class<T>(Action<ClassMap<T>> setup)
        {
            var provider = new ClassMap<T>();

            setup(provider);

            return ((IMappingProvider)provider).GetClassMapping();
        }

        public static SubclassMapping map_as_subclass<T>(Action<SubclassMap<T>> setup)
        {
            var provider = new SubclassMap<T>();
            var mapping = new SubclassMapping();

            setup(provider);

            ((IIndeterminateSubclassMappingProvider)provider).GetSubclassMapping(mapping);

            return mapping;
        }
    }
}