using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.Automapping
{
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