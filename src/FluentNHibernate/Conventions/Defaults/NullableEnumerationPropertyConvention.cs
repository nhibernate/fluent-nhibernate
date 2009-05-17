using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    ///// <summary>
    ///// Nullable enum convention. Same behavior as <see cref="EnumerationPropertyConvention"/> but sets the
    ///// property to nullable aswell.
    ///// </summary>
    //public class NullableEnumerationPropertyConvention : IPropertyConvention
    //{
    //    public bool Accept(IProperty target)
    //    {
    //        var type = target.PropertyType;

    //        return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && type.GetGenericArguments()[0].IsEnum;
    //    }

    //    public void Apply(IProperty target)
    //    {
    //        var enumerationType = target.PropertyType.GetGenericArguments()[0];
    //        var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(enumerationType);

    //        target.CustomTypeIs(mapperType);
    //        target.Nullable();
    //    }
    //}
}