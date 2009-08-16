using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class HibernateMappingPart : IHibernateMappingProvider
    {
        private readonly CascadeExpression<HibernateMappingPart> defaultCascade;
        private readonly AccessStrategyBuilder<HibernateMappingPart> defaultAccess;
        private readonly AttributeStore<HibernateMapping> attributes = new AttributeStore<HibernateMapping>();
        private bool nextBool = true;

        public HibernateMappingPart()
        {
            defaultCascade = new CascadeExpression<HibernateMappingPart>(this, value => attributes.Set(x => x.DefaultCascade, value));
            defaultAccess = new AccessStrategyBuilder<HibernateMappingPart>(this, value => attributes.Set(x => x.DefaultAccess, value));
        }

        public HibernateMappingPart Schema(string schema)
        {
            attributes.Set(x => x.Schema, schema);
            return this;
        }

        public CascadeExpression<HibernateMappingPart> DefaultCascade
        {
            get { return defaultCascade; }
        }

        public AccessStrategyBuilder<HibernateMappingPart> DefaultAccess
        {
            get { return defaultAccess; }
        }

        public HibernateMappingPart AutoImport()
        {
            attributes.Set(x => x.AutoImport, nextBool);
            nextBool = true;
            return this;
        }

        public HibernateMappingPart DefaultLazy()
        {
            attributes.Set(x => x.DefaultLazy, nextBool);
            nextBool = true;
            return this;
        }

        public HibernateMappingPart Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public HibernateMappingPart Catalog(string catalog)
        {
            attributes.Set(x => x.Catalog, catalog);
            return this;
        }

        public HibernateMappingPart Namespace(string ns)
        {
            attributes.Set(x => x.Namespace, ns);
            return this;
        }

        public HibernateMappingPart Assembly(string assembly)
        {
            attributes.Set(x => x.Assembly, assembly);
            return this;
        }

        HibernateMapping IHibernateMappingProvider.GetHibernateMapping()
        {
            return new HibernateMapping(attributes.CloneInner());
        }
    }
}