using System.Collections.Generic;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IGeneratorInspector
    {
        string Class { get; }
        IDictionary<string, string> Params { get; }
    }
}