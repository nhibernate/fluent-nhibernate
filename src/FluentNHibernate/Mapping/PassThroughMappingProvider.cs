using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class PassThroughMappingProvider : IMappingProvider, ICollectionMappingProvider, IManyToOneMappingProvider, ICompositeElementMappingProvider, IPropertyMappingProvider, IFilterMappingProvider
    {
        private readonly object mapping;

        public PassThroughMappingProvider(object mapping)
        {
            this.mapping = mapping;
        }

        public ClassMapping GetClassMapping()
        {
            return (ClassMapping)mapping;
        }

        public ICollectionMapping GetCollectionMapping()
        {
            return (ICollectionMapping)mapping;
        }

        public ManyToOneMapping GetManyToOneMapping()
        {
            return (ManyToOneMapping)mapping;
        }

        public HibernateMapping GetHibernateMapping()
        {
            return new HibernateMapping();
        }

        public IEnumerable<Member> GetIgnoredProperties()
        {
            return new Member[0];
        }

        public CompositeElementMapping GetCompositeElementMapping()
        {
            return (CompositeElementMapping)mapping;
        }

        public PropertyMapping GetPropertyMapping()
        {
            return (PropertyMapping)mapping;
        }

        public FilterMapping GetFilterMapping()
        {
            return (FilterMapping)mapping;
        }
    }
}