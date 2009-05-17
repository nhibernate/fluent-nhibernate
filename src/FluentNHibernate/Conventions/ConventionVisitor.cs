using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions
{
    public class ConventionVisitor : DefaultMappingModelVisitor
    {
        private readonly IConventionFinder finder;

        public ConventionVisitor(IConventionFinder finder)
        {
            this.finder = finder;
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            var dsl = new PropertyDsl(propertyMapping);
            var conventions = finder.Find<IPropertyConvention>();

            foreach (var convention in conventions)
            {
                var criteria = new ConcreteAcceptanceCriteria<IPropertyInspector>();

                convention.Accept(criteria);

                if (criteria.Matches(dsl))
                    convention.Apply(dsl);
            }
        }
    }
}
