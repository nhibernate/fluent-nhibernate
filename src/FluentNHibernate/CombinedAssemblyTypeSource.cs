using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentNHibernate
{
    /// <summary>
    /// Provides types for mapping from multiple assemblies
    /// </summary>
    public class CombinedAssemblyTypeSource : ITypeSource
    {
        readonly IEnumerable<Assembly> sources;

        public CombinedAssemblyTypeSource(IEnumerable<Assembly> sources)
        {
            this.sources = sources;
        }

        public IEnumerable<Type> GetTypes()
        {
            return sources.SelectMany(x => x.GetExportedTypes()).ToArray();
        }
    }
}