using System.Linq;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Nullable enum convention. Same behavior as <see cref="EnumerationPropertyConvention"/> but sets the
    /// property to nullable aswell.
    /// </summary>
    public class NullableEnumerationPropertyConvention : IPropertyConvention, IConventionAcceptance<IPropertyInspector>
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria
                .Expect(x => x.Type.IsGenericType)
                .Expect(x => x.Type.IsNullable)
                .Expect(x => x.Type.GenericArguments.First().IsEnum);
        }

        public void Apply(IPropertyInstance instance)
        {
            var enumerationType = instance.Type.GetGenericArguments()[0];
            var mapperType = typeof(GenericEnumMapper<>).MakeGenericType(enumerationType);

            instance.CustomType(mapperType);
            instance.Nullable();
        }
    }
}