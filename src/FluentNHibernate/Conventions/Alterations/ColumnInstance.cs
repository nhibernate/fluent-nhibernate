using System;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations
{
    public class ColumnInstance : ColumnInspector, IColumnInstance
    {
        private readonly ColumnMapping mapping;

        public ColumnInstance(Type parentType, ColumnMapping mapping)
            : base(parentType, mapping)
        {
            this.mapping = mapping;
        }
    }
}