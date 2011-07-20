using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class KeyPropertyInstance : KeyPropertyInspector, IKeyPropertyInstance
    {
        private readonly KeyPropertyMapping mapping;

        public KeyPropertyInstance(KeyPropertyMapping mapping)
            : base(mapping)
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
        public new void Length(int length)
        {
            if (!mapping.IsSpecified("Length"))
                mapping.Length = length;
        }
    }
}
