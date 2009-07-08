using System;
using System.Collections;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

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

        public override void ProcessId(IdMapping idMapping)
        {
            var conventions = finder.Find<IIdConvention>();

            Apply<IIdentityInspector, IIdentityAlteration, IIdentityInstance>(conventions,
                new IdentityInstance(idMapping));
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            var conventions = finder.Find<IClassConvention>();

            currentType = classMapping.Type;

            Apply<IClassInspector, IClassAlteration, IClassInstance>(conventions,
                new ClassInstance(classMapping));
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            var conventions = finder.Find<IPropertyConvention>();

            Apply<IPropertyInspector, IPropertyAlteration, IPropertyInstance>(conventions,
                new PropertyInstance(propertyMapping));
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            var conventions = finder.Find<IColumnConvention>();

            Apply<IColumnInspector, IColumnAlteration, IColumnInstance>(conventions,
                new ColumnInstance(currentType, columnMapping));
        }

        protected override void ProcessCollection(ICollectionMapping mapping)
        {
            var generalConventions = finder.Find<ICollectionConvention>();

            Apply<ICollectionInspector, ICollectionAlteration, ICollectionInstance>(generalConventions,
                new CollectionInstance(mapping));

            if (mapping.Relationship is ManyToManyMapping)
            {
                var conventions = finder.Find<IHasManyToManyConvention>();

                Apply<IManyToManyCollectionInspector, IManyToManyCollectionAlteration, IManyToManyCollectionInstance>(conventions,
                    new ManyToManyCollectionInstance(mapping));
            }
            else
            {
                var conventions = finder.Find<IHasManyConvention>();

                Apply<IOneToManyCollectionInspector, IOneToManyCollectionAlteration, IOneToManyCollectionInstance>(conventions,
                    new OneToManyCollectionInstance(mapping));
            }
        }

        public override void ProcessManyToOne(ManyToOneMapping mapping)
        {
            var conventions = finder.Find<IReferenceConvention>();

            Apply<IManyToOneInspector, IManyToOneAlteration, IManyToOneInstance>(conventions,
                new ManyToOneInstance(mapping));
        }

        private void Apply<TInspector, TAlteration, TInstance>(IEnumerable conventions, TInstance instance)
            where TInspector : IInspector
            where TAlteration : IAlteration
            where TInstance : TInspector, TAlteration
        {
            foreach (IConvention<TInspector, TAlteration, TInstance> convention in conventions)
            {
                var criteria = new ConcreteAcceptanceCriteria<TInspector>();

                convention.Accept(criteria);

                if (criteria.Matches(instance))
                    convention.Apply(instance);
            }
        }
    }
}
