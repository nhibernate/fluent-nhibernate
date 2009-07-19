using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class AnyInspector : IAnyInspector
    {
        private readonly AnyMapping mapping;

        public AnyInspector(AnyMapping mapping)
        {
            this.mapping = mapping;
        }
    }
}