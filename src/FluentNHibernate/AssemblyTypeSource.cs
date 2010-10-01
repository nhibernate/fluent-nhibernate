using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using FluentNHibernate.Diagnostics;

namespace FluentNHibernate
{
    /// <summary>
    /// Facade over an assembly for retrieving type instances.
    /// </summary>
    public class AssemblyTypeSource : ITypeSource
    {
        private readonly Assembly source;

        public AssemblyTypeSource(Assembly source)
        {
            this.source = source;
        }

        public IEnumerable<Type> GetTypes()
        {
            return source.GetExportedTypes().OrderBy(x => x.FullName);
        }

        public void LogSource(IDiagnosticLogger logger)
        {
            logger.LoadedFluentMappingsFromSource(this);
        }

        public string GetIdentifier()
        {
            return source.GetName().FullName;
        }
    }
}