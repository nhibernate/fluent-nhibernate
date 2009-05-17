using System.Collections;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions
{
    public class ConventionVisitor : DefaultMappingModelVisitor
    {
        private readonly IConventionFinder finder;

        public ConventionVisitor(IConventionFinder finder)
        {
            this.finder = finder;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            var dsl = new ClassDsl(classMapping);
            var conventions = finder.Find<IClassConvention>();

            Apply<IClassInspector, IClassAlteration>(conventions, dsl, dsl);
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            var dsl = new PropertyDsl(propertyMapping);
            var conventions = finder.Find<IPropertyConvention>();

            Apply<IPropertyInspector, IPropertyAlteration>(conventions, dsl, dsl);
        }

        private void Apply<TInspector, TAlteration>(IEnumerable conventions, TInspector inspection, TAlteration alteration)
            where TInspector : IInspector
        {
            foreach (IConvention<TInspector, TAlteration> convention in conventions)
            {
                var criteria = new ConcreteAcceptanceCriteria<TInspector>();

                convention.Accept(criteria);

                if (criteria.Matches(inspection))
                    convention.Apply(alteration, inspection);
            }
        }
    }
}
