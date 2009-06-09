using System.Linq;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Nullable enum convention. Same behavior as <see cref="EnumerationPropertyConvention"/> but sets the
    /// property to nullable aswell.
    /// </summary>
    public class NullableEnumerationPropertyConvention : IPropertyConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> acceptance)
        {
            acceptance
                .Expect(x => x.Type.IsGenericType)
                .Expect(x => x.Type.IsNullable)
                .Expect(x => x.Type.GenericArguments.First().IsEnum);
        }

        public void Apply(IPropertyAlteration alteration, IPropertyInspector inspector)
        {
            var enumerationType = inspector.Type.GetGenericArguments()[0];
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(enumerationType);

            alteration.CustomTypeIs(mapperType);
            alteration.Nullable();
        }
    }
}