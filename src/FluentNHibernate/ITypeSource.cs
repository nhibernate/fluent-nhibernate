using System;
using System.Collections.Generic;

namespace FluentNHibernate
{
    /// <summary>
    /// A source for Type instances, acts as a facade for an Assembly or as an alternative Type provider.
    /// </summary>
    public interface ITypeSource
    {
        IEnumerable<Type> GetTypes();
    }
}