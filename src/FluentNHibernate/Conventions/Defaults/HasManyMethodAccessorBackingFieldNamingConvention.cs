using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default HasMany backing field naming convention
    /// </summary>
    public class HasManyMethodAccessorBackingFieldNamingConvention
        : BaseMethodAccessorBackingFieldNamingConvention<IOneToManyPart>, IHasManyConvention
    {}
}