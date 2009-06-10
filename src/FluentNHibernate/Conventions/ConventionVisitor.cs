using System;
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

        private Type currentType;

        public ConventionVisitor(IConventionFinder finder)
        {
            this.finder = finder;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            var dsl = new ClassDsl(classMapping);
            var conventions = finder.Find<IClassConvention>();

            currentType = classMapping.Type;

            Apply<IClassInspector, IClassAlteration>(conventions, dsl, dsl);
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            var conventions = finder.Find<IPropertyConvention>();

            Apply<IPropertyInspector, IPropertyAlteration>(conventions,
                new PropertyInspector(propertyMapping),
                new PropertyAlteration(propertyMapping));
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            var conventions = finder.Find<IColumnConvention>();

            Apply<IColumnInspector, IColumnAlteration>(conventions,
                new ColumnInspector(currentType, columnMapping),
                new ColumnAlteration(columnMapping));
        }

        public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            var conventions = finder.Find<IReferenceConvention>();

            Apply<IManyToOneInspector, IManyToOneAlteration>(conventions,
                new ManyToOneInspector(manyToOneMapping),
                new ManyToOneAlteration(manyToOneMapping));
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
