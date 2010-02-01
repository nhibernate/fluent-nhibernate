using System.Collections.Generic;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IJoinInstance : IJoinInspector
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IJoinInstance Not { get; }
        new IFetchInstance Fetch { get; }
        new void Inverse();
        new IKeyInstance Key { get; }
        new void Optional();
        new void Schema(string schema);
        void Table(string table);
        new void Catalog(string catalog);
        new void Subselect(string subselect);
    }
}