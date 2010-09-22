using System;
using System.Collections.Generic;

namespace FluentNHibernate.Diagnostics
{
    public interface IDiagnosticLogger
    {
        void Flush();
        void FluentMappingDiscovered(Type type);
        void ConventionDiscovered(Type type);
        void LoadedFluentMappingsFromSource(ITypeSource source);
        void LoadedConventionsFromSource(ITypeSource source);
        void AutomappingSkippedType(Type type, string reason);
        void AutomappingCandidateTypes(IEnumerable<Type> types);
        void BeginAutomappingType(Type type);
    }
}