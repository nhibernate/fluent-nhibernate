using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.Diagnostics;

public class DiagnosticResults
{

    public DiagnosticResults(IEnumerable<ScannedSource> scannedSources, IEnumerable<Type> fluentMappings, IEnumerable<Type> conventions, IEnumerable<SkippedAutomappingType> automappingSkippedTypes, IEnumerable<Type> automappingCandidateTypes, IEnumerable<AutomappingType> automappingTypes)
    {
        FluentMappings = fluentMappings.ToArray();
        ScannedSources = scannedSources.ToArray();
        Conventions = conventions.ToArray();
        AutomappingSkippedTypes = automappingSkippedTypes.ToArray();
        AutomappingCandidateTypes = automappingCandidateTypes.ToArray();
        AutomappedTypes = automappingTypes.ToArray();
    }

    public IEnumerable<Type> FluentMappings { get; }
    public IEnumerable<ScannedSource> ScannedSources { get; }
    public IEnumerable<Type> Conventions { get; }
    public IEnumerable<SkippedAutomappingType> AutomappingSkippedTypes { get; }
    public IEnumerable<Type> AutomappingCandidateTypes { get; }
    public IEnumerable<AutomappingType> AutomappedTypes { get; }
}
