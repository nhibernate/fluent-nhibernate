using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IJoinInstance : IJoinInspector
    {
        IJoinInstance Not { get; }
        void Fetch(Fetch fetch);
        void Inverse();
        new IKeyInstance Key { get; }
        void Optional();
        void Schema(string schema);
        void Table(string table);
        void Catalog(string catalog);
        void Subselect(string subselect);
    }
}