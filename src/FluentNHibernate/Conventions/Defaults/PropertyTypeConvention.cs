using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;

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
                .Expect(x => x.Type, Is.Not.Set)
                .Expect(x => x.Type.IsEnum == false);
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomTypeIs(instance.Type);
        }
    }
}