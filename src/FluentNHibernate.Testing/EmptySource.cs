using System;
using System.Collections.Generic;
using FluentNHibernate.Diagnostics;

namespace FluentNHibernate.Testing
{
    internal class EmptySource : ITypeSource
    {
        public IEnumerable<Type> GetTypes()
        {
            return Array.Empty<Type>();
        }

        public void LogSource(IDiagnosticLogger logger)
        {
            logger.LoadedFluentMappingsFromSource(this);
        }

        public string GetIdentifier()
        {
            return "EmptySource";
        }
    }
}