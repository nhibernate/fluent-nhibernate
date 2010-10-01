using System;
using System.Collections.Generic;

namespace FluentNHibernate.Diagnostics
{
    public class NullDiagnosticsLogger : IDiagnosticLogger
    {
        public void Flush()
        {}

        public void FluentMappingDiscovered(Type type)
        {}

        public void ConventionDiscovered(Type type)
        {}

        public void LoadedFluentMappingsFromSource(ITypeSource source)
        {}

        public void LoadedConventionsFromSource(ITypeSource source)
        {}

        public void AutomappingSkippedType(Type type, string reason)
        {}

        public void AutomappingCandidateTypes(IEnumerable<Type> types)
        {}

        public void BeginAutomappingType(Type type)
        {}
    }
}