using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Diagnostics;

namespace FluentNHibernate;

/// <summary>
/// Facade over an assembly for retrieving type instances.
/// </summary>
public class AssemblyTypeSource(Assembly source) : ITypeSource
{
    readonly Assembly source = source ?? throw new ArgumentNullException(nameof(source));

    #region ITypeSource Members

    public IEnumerable<Type> GetTypes()
    {
        return source.GetTypes().OrderBy(x => x.FullName);
    }

    public void LogSource(IDiagnosticLogger logger)
    {
        if (logger is null) throw new ArgumentNullException(nameof(logger));

        logger.LoadedFluentMappingsFromSource(this);
    }

    public string GetIdentifier()
    {
        return source.GetName().FullName;
    }

    #endregion

    public override int GetHashCode()
    {
        return source.GetHashCode();
    }
}
