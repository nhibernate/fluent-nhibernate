using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Sets the type attribute on property mappings
    /// </summary>
    public class PropertyTypeConvention : IPropertyConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> acceptance)
        {
            acceptance
                .Expect(x => x.CustomType, Is.Not.Set)
                .Expect(x => x.PropertyType.IsEnum == false);
        }

        public void Apply(IPropertyInspector target)
        {
            //target.CustomTypeIs(target.PropertyType);
        }
    }
}