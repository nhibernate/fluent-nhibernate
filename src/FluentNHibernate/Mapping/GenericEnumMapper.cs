using System;
using NHibernate.Type;

namespace ShadeTree.DomainModel.Mapping
{
    public class GenericEnumMapper<TEnum> : EnumStringType
    {
        public GenericEnumMapper()
            : base(typeof (TEnum))
        {
        }
    }
}