using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class FormulaConvention : IPropertyConvention, IReferenceConvention
    {
        public const string FormulaValue = "select x from y";

        public void Apply(IPropertyInstance instance)
        {
            instance.Formula(FormulaValue);
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.Formula(FormulaValue);
        }
    }
}