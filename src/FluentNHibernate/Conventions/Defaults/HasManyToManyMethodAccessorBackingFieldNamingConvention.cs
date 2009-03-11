using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class HasManyToManyMethodAccessorBackingFieldNamingConvention
        : BaseMethodAccessorBackingFieldNamingConvention<IManyToManyPart>, IHasManyToManyConvention
    {}
}