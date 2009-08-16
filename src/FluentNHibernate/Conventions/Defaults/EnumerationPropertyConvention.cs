using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Specifies a custom type (of <see cref="GenericEnumMapper{TEnum}"/>) for any properties
    /// that are an enum.
    /// </summary>
    public class EnumerationPropertyConvention : IPropertyConvention, IConventionAcceptance<IPropertyInspector>
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria
                .Expect(x => x.Type, Is.Not.Set)
                .Expect(x => x.Type.IsEnum);
        }

        public void Apply(IPropertyInstance instance)
        {
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(instance.Type.GetUnderlyingSystemType());

            instance.CustomType(mapperType);
        }
    }
}