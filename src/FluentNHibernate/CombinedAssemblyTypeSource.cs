using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Diagnostics;

namespace FluentNHibernate
{
    /// <summary>
    /// Provides types for mapping from multiple assemblies
    /// </summary>
    public class CombinedAssemblyTypeSource : ITypeSource
    {
        readonly AssemblyTypeSource[] sources;

        public CombinedAssemblyTypeSource(IEnumerable<Assembly> sources)
            : this(sources.Select(x => new AssemblyTypeSource(x)))
        {}

        public CombinedAssemblyTypeSource(IEnumerable<AssemblyTypeSource> sources)
        {
            this.sources = sources.ToArray();
        }

        public IEnumerable<Type> GetTypes()
        {
            return sources
                .SelectMany(x => x.GetTypes())
                .ToArray();
        }

        public void LogSource(IDiagnosticLogger logger)
        {
            foreach (var source in sources)
                source.LogSource(logger);
        }

        public string GetIdentifier()
        {
            return "Combined source";
        }
    }
}