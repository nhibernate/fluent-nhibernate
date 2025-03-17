using System.Diagnostics;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping;

public class HibernateMappingPart : IHibernateMappingProvider
{
    readonly AttributeStore attributes = new AttributeStore();
    bool nextBool = true;

    public HibernateMappingPart()
    {
        DefaultCascade = new CascadeExpression<HibernateMappingPart>(this, value =>
        {
            var current = attributes.Get("DefaultCascade") as string;
            attributes.Set("DefaultCascade", Layer.UserSupplied, current is null ? value : string.Format("{0},{1}", current, value));
        });
        DefaultAccess = new AccessStrategyBuilder<HibernateMappingPart>(this, value => attributes.Set("DefaultAccess", Layer.UserSupplied, value));
    }

    public HibernateMappingPart Schema(string schema)
    {
        attributes.Set("Schema", Layer.UserSupplied, schema);
        return this;
    }

    public CascadeExpression<HibernateMappingPart> DefaultCascade { get; }

    public AccessStrategyBuilder<HibernateMappingPart> DefaultAccess { get; }

    public HibernateMappingPart AutoImport()
    {
        attributes.Set("AutoImport", Layer.UserSupplied, nextBool);
        nextBool = true;
        return this;
    }

    public HibernateMappingPart DefaultLazy()
    {
        attributes.Set("DefaultLazy", Layer.UserSupplied, nextBool);
        nextBool = true;
        return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        attributes.Set("Catalog", Layer.UserSupplied, catalog);
        return this;
    }

    public HibernateMappingPart Namespace(string ns)
    {
        attributes.Set("Namespace", Layer.UserSupplied, ns);
        return this;
    }

    public HibernateMappingPart Assembly(string assembly)
    {
        attributes.Set("Assembly", Layer.UserSupplied, assembly);
        return this;
    }

    HibernateMapping IHibernateMappingProvider.GetHibernateMapping()
    {
        return new HibernateMapping(attributes.Clone());
    }
}
