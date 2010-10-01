using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ListInstance : ListInspector, IListInstance
    {
        private readonly ListMapping mapping;
        public ListInstance(ListMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        new public IIndexInstanceBase Index
        {
            get
            {
                var index = mapping.Index as IndexMapping;

                if (index != null)
                {
                    if (index.IsManyToMany)
                        return new IndexManyToManyInstance(index);
                    
                    return new IndexInstance(index);
                }

                throw new InvalidOperationException("IIndexMapping is not a valid type for building an Index Instance ");
            }
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
