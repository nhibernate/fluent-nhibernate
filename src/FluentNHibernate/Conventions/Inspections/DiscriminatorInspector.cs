using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class DiscriminatorInspector : IDiscriminatorInspector
    {
        private readonly DiscriminatorMapping mapping;

        public DiscriminatorInspector(DiscriminatorMapping mapping)
        {
            this.mapping = mapping;
        }

        public bool Insert
        {
            get { return mapping.Insert; }
        }
    }
}