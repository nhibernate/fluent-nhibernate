using System.Linq;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Testing.AutoMap
{
    public class XXAppenderPropertyConvention : IPropertyConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> acceptance)
        {}

        public void Apply(IPropertyAlteration alteration, IPropertyInspector inspector)
        {
            alteration.ColumnName(inspector.Columns.First().Name + "XX");
        }
    }
}
