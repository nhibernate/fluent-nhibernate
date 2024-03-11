using System;
using System.Collections.Generic;
using FluentNHibernate.Diagnostics;

namespace FluentNHibernate.Specs.Automapping.Fixtures;

internal class StubTypeSource(params Type[] types) : ITypeSource
{
    private readonly IEnumerable<Type> types = types;

    public IEnumerable<Type> GetTypes()
    {
        return types;
    }

    public void LogSource(IDiagnosticLogger logger)
    {
        logger.LoadedFluentMappingsFromSource(this);
    }

    public string GetIdentifier()
    {
        return "StubTypeSource";
    }
}
