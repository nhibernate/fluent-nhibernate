using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default HasManyToMany backing field naming convention
    /// </summary>
    public class HasManyToManyMethodAccessorBackingFieldNamingConvention
        : BaseMethodAccessorBackingFieldNamingConvention<IManyToManyPart>, IHasManyToManyConvention
    {}
}