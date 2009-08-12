using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class ColumnInstance : ColumnInspector, IColumnInstance
    {
        private readonly ColumnMapping mapping;

        public ColumnInstance(Type parentType, ColumnMapping mapping)
            : base(parentType, mapping)
        {
            this.mapping = mapping;
        }

        public new void Length(int length)
        {
            if (!mapping.IsSpecified(x => x.Length))
                mapping.Length = length;
        }
    }
}