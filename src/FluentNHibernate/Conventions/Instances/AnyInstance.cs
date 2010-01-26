using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class AnyInstance : AnyInspector, IAnyInstance
    {
        private readonly AnyMapping mapping;

        public AnyInstance(AnyMapping mapping) : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }
    }
}