using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.Diagnostics;

public class DiagnosticResults(
    IEnumerable<ScannedSource> scannedSources,
    IEnumerable<Type> fluentMappings,
    IEnumerable<Type> conventions,
    IEnumerable<SkippedAutomappingType> automappingSkippedTypes,
    IEnumerable<Type> automappingCandidateTypes,
    IEnumerable<AutomappingType> automappingTypes)
{
    public IEnumerable<Type> FluentMappings { get; } = fluentMappings.ToArray();
    public IEnumerable<ScannedSource> ScannedSources { get; } = scannedSources.ToArray();
    public IEnumerable<Type> Conventions { get; } = conventions.ToArray();
    public IEnumerable<SkippedAutomappingType> AutomappingSkippedTypes { get; } = automappingSkippedTypes.ToArray();
    public IEnumerable<Type> AutomappingCandidateTypes { get; } = automappingCandidateTypes.ToArray();
    public IEnumerable<AutomappingType> AutomappedTypes { get; } = automappingTypes.ToArray();
}
