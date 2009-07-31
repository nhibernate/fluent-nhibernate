using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class HibernateMappingPart : IHibernateMappingProvider
    {
        private readonly CascadeExpression<HibernateMappingPart> defaultCascade;
        private readonly AccessStrategyBuilder<HibernateMappingPart> defaultAccess;
        private readonly HibernateMapping mapping = new HibernateMapping();
        private bool nextBool = true;

        public HibernateMappingPart()
        {
            defaultCascade = new CascadeExpression<HibernateMappingPart>(this, value => mapping.DefaultCascade = value);
            defaultAccess = new AccessStrategyBuilder<HibernateMappingPart>(this, value => mapping.DefaultAccess = value);
        }

        public HibernateMappingPart Schema(string schema)
        {
            mapping.Schema = schema;
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
            mapping.AutoImport = nextBool;
            nextBool = true;
            return this;
        }

        public HibernateMappingPart DefaultLazy()
        {
            mapping.DefaultLazy = nextBool;
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
            mapping.Catalog = catalog;
            return this;
        }

        public HibernateMappingPart Namespace(string ns)
        {
            mapping.Namespace = ns;
            return this;
        }

        public HibernateMappingPart Assembly(string assembly)
        {
            mapping.Assembly = assembly;
            return this;
        }

        HibernateMapping IHibernateMappingProvider.GetHibernateMapping()
        {
            return mapping;
        }
    }
}