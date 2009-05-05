using NHibernate.Type;

namespace FluentNHibernate.Mapping
{
    public class GenericEnumMapper<TEnum> : EnumStringType
    {
        public GenericEnumMapper()
            : base(typeof (TEnum))
        {
        }
    }
}