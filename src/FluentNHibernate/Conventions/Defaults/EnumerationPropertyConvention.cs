using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Specifies a custom type (of <see cref="GenericEnumMapper{TEnum}"/>) for any properties
    /// that are an enum.
    /// </summary>
    public class EnumerationPropertyConvention : IPropertyConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> acceptance)
        {
            acceptance
                .Expect(x => x.CustomType, Is.Not.Set)
                .Expect(x => x.PropertyType.IsEnum);
        }

        public void Apply(IPropertyAlteration alteration, IPropertyInspector inspector)
        {
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(inspector.PropertyType);

            alteration.CustomTypeIs(mapperType);
        }
    }
}