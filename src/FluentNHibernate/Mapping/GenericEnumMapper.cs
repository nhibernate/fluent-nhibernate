using System;
using NHibernate.Type;

namespace FluentNHibernate.Mapping
{
    [Serializable]
    public class GenericEnumMapper<TEnum> : EnumStringType
    {
        public GenericEnumMapper()
            : base(typeof(TEnum))
        {
        }
    }
}