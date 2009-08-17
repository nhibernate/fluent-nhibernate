using System;
using System.Collections.Generic;

namespace FluentNHibernate.Automapping
{
    /// <summary>
    /// A source for Type instances, used for locating types that should be
    /// automapped.
    /// </summary>
    public interface ITypeSource
    {
        IEnumerable<Type> GetTypes();
    }
}