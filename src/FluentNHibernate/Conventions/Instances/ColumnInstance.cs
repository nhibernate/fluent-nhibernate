using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances;

public class ColumnInstance(Type parentType, ColumnMapping mapping)
    : ColumnInspector(parentType, mapping), IColumnInstance
{
    readonly ColumnMapping mapping = mapping;

    public new void Length(int length)
    {
        mapping.Set(x => x.Length, Layer.Conventions, length);
    }

    public new void Index(string indexname)
    {
        mapping.Set(x => x.Index, Layer.Conventions, indexname);
    }

    public new void Default(string defaultvalue)
    {
        mapping.Set(x => x.Default, Layer.Conventions, defaultvalue);
    }
}
