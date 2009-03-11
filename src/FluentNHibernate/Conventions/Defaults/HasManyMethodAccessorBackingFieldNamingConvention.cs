using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class HasManyMethodAccessorBackingFieldNamingConvention
        : BaseMethodAccessorBackingFieldNamingConvention<IOneToManyPart>, IHasManyConvention
    {}
}