using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class MappingProviderStore
    {
        public IList<IPropertyMappingProvider> Properties { get; set; }
        public IList<IComponentMappingProvider> Components { get; set; }
        public IList<IOneToOneMappingProvider> OneToOnes { get; set; }
        public Dictionary<Type, ISubclassMappingProvider> Subclasses { get; set; }
        public IList<ICollectionMappingProvider> Collections { get; set; }
        public IList<IManyToOneMappingProvider> References { get; set; }
        public IList<IAnyMappingProvider> Anys { get; set; }
        public IList<IFilterMappingProvider> Filters { get; set; }
        public IList<IStoredProcedureMappingProvider> StoredProcedures { get; set; }
        public IList<IJoinMappingProvider> Joins { get; set; }

        public IIdentityMappingProvider Id { get; set; }
        public ICompositeIdMappingProvider CompositeId { get; set; }
        public INaturalIdMappingProvider NaturalId { get; set; }
        public IVersionMappingProvider Version { get; set; }
        public IDiscriminatorMappingProvider Discriminator { get; set; }
        public TuplizerMapping TuplizerMapping { get; set; }

        public MappingProviderStore()
        {
            Properties = new List<IPropertyMappingProvider>();
            Components = new List<IComponentMappingProvider>();
            OneToOnes = new List<IOneToOneMappingProvider>();
            Subclasses = new Dictionary<Type, ISubclassMappingProvider>();
            Collections = new List<ICollectionMappingProvider>();
            References = new List<IManyToOneMappingProvider>();
            Anys = new List<IAnyMappingProvider>();
            Filters = new List<IFilterMappingProvider>();
            StoredProcedures = new List<IStoredProcedureMappingProvider>();
            Joins = new List<IJoinMappingProvider>();
        }
    }
}