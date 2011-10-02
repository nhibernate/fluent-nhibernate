using System;
using System.Collections.Generic;

namespace FluentNHibernate.Diagnostics
{
    public class DefaultDiagnosticLogger : IDiagnosticLogger
    {
        readonly IDiagnosticMessageDispatcher dispatcher;
        readonly List<ScannedSource> scannedSources = new List<ScannedSource>();
        readonly List<Type> classMaps = new List<Type>();
        readonly List<Type> conventions = new List<Type>();
        readonly List<SkippedAutomappingType> automappingSkippedTypes = new List<SkippedAutomappingType>();
        readonly List<Type> automappingCandidateTypes = new List<Type>();
        readonly List<AutomappingType> automappingTypes = new List<AutomappingType>();
        bool isDirty;

        public DefaultDiagnosticLogger(IDiagnosticMessageDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void Flush()
        {
            if (!isDirty) return;

            var results = BuildResults();

            dispatcher.Publish(results);
        }

        DiagnosticResults BuildResults()
        {
            return new DiagnosticResults(scannedSources, classMaps, conventions, automappingSkippedTypes, automappingCandidateTypes, automappingTypes);
        }

        public void FluentMappingDiscovered(Type type)
        {
            isDirty = true;
            classMaps.Add(type);
        }

        public void ConventionDiscovered(Type type)
        {
            isDirty = true;
            conventions.Add(type);
        }

        public void LoadedFluentMappingsFromSource(ITypeSource source)
        {
            isDirty = true;
            scannedSources.Add(new ScannedSource
            {
                Identifier = source.GetIdentifier(),
                Phase = ScanPhase.FluentMappings
            });
        }

        public void LoadedConventionsFromSource(ITypeSource source)
        {
            isDirty = true;
            scannedSources.Add(new ScannedSource
            {
                Identifier = source.GetIdentifier(),
                Phase = ScanPhase.Conventions
            });
        }

        public void AutomappingSkippedType(Type type, string reason)
        {
            isDirty = true;
            automappingSkippedTypes.Add(new SkippedAutomappingType
            {
                Type = type,
                Reason = reason
            });
        }

        public void AutomappingCandidateTypes(IEnumerable<Type> types)
        {
            isDirty = true;
            automappingCandidateTypes.AddRange(types);
        }

        public void BeginAutomappingType(Type type)
        {
            isDirty = true;
            automappingTypes.Add(new AutomappingType
            {
                Type = type
            });
        }
    }
}