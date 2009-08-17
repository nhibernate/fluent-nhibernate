using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class HibernateMappingInstance : HibernateMappingInspector, IHibernateMappingInstance
    {
        private readonly HibernateMapping mapping;
        private bool nextBool = true;

        public HibernateMappingInstance(HibernateMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new void Catalog(string catalog)
        {
            if (!mapping.IsSpecified(x => x.Catalog))
                mapping.Catalog = catalog;
        }

        public new void Schema(string schema)
        {
            if (!mapping.IsSpecified(x => x.Schema))
                mapping.Schema = schema;
        }

        public IHibernateMappingInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new void DefaultLazy()
        {
            if (mapping.IsSpecified(x => x.DefaultLazy))
                return;

            mapping.DefaultLazy = nextBool;
            nextBool = true;
        }

        public new ICascadeInstance DefaultCascade
        {
            get
            {
                return new CascadeInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.DefaultCascade))
                        mapping.DefaultCascade = value;
                });
            }
        }

        public new IAccessInstance DefaultAccess
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.DefaultAccess))
                        mapping.DefaultAccess = value;
                });
            }
        }
    }
}