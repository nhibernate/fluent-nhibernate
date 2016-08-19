using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Providers
{
    /// <summary>
    /// This collection optimizes search for already mapped types in subclasses providers. Matters in models with huge inheritance trees.
    /// </summary>
    public class IndeterminateSubclassMappingProviderCollection : IIndeterminateSubclassMappingProviderCollection
    {
        private readonly List<IIndeterminateSubclassMappingProvider> providers = new List<IIndeterminateSubclassMappingProvider>(); 
        private readonly HashSet<Type> mappedTypes = new HashSet<Type>(); 

        public IEnumerator<IIndeterminateSubclassMappingProvider> GetEnumerator()
        {
            return providers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IIndeterminateSubclassMappingProvider item)
        {
            providers.Add(item);
            mappedTypes.Add(item.EntityType);
        }

        public bool IsTypeMapped(Type type)
        {
            return mappedTypes.Contains(type);
        }
    }
}