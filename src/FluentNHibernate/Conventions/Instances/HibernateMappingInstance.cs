using System.Diagnostics;
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
            mapping.Set(x => x.Catalog, Layer.Conventions, catalog);
        }

        public new void Schema(string schema)
        {
            mapping.Set(x => x.Schema, Layer.Conventions, schema);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            mapping.Set(x => x.DefaultLazy, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void AutoImport()
        {
            mapping.Set(x => x.AutoImport, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new ICascadeInstance DefaultCascade
        {
            get { return new CascadeInstance(value => mapping.Set(x => x.DefaultCascade, Layer.Conventions, value)); }
        }

        public new IAccessInstance DefaultAccess
        {
            get { return new AccessInstance(value => mapping.Set(x => x.DefaultAccess, Layer.Conventions, value)); }
        }
    }
}