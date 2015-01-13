using System;
using System.Collections.Generic;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IIndeterminateSubclassMappingProviderCollection : IEnumerable<IIndeterminateSubclassMappingProvider>
    {        
        void Add(IIndeterminateSubclassMappingProvider item);
        bool IsTypeMapped(Type type);
    }
}