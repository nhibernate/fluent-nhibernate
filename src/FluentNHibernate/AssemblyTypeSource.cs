using System;
using System.Collections.Generic;
using System.Reflection;

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
            return source.GetExportedTypes();
        }
    }
}