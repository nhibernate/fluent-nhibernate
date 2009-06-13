using System;
using System.Collections;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

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
            var conventions = finder.Find<IClassConvention>();

            currentType = classMapping.Type;

            Apply<IClassInspector, IClassAlteration>(conventions,
                new ClassInspector(classMapping),
                new ClassAlteration(classMapping));
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

        protected override void ProcessCollection(ICollectionMapping mapping)
        {
            var generalConventions = finder.Find<ICollectionConvention>();

            Apply<ICollectionInspector, ICollectionAlteration>(generalConventions,
                new CollectionInspector(mapping),
                new CollectionAlteration(mapping));

            if (mapping.Relationship is ManyToManyMapping)
            {
                var conventions = finder.Find<IHasManyToManyConvention>();

                Apply<IManyToManyCollectionInspector, IManyToManyCollectionAlteration>(conventions,
                    new CollectionInspector(mapping),
                    new CollectionAlteration(mapping));
            }
            else
            {
                var conventions = finder.Find<IHasManyConvention>();

                Apply<IOneToManyCollectionInspector, IOneToManyCollectionAlteration>(conventions,
                    new CollectionInspector(mapping),
                    new CollectionAlteration(mapping));
            }
        }

        public override void ProcessManyToOne(ManyToOneMapping mapping)
        {
            var conventions = finder.Find<IReferenceConvention>();

            Apply<IManyToOneInspector, IManyToOneAlteration>(conventions,
                new ManyToOneInspector(mapping),
                new ManyToOneAlteration(mapping));
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
