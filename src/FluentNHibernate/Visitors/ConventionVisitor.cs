using System;
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using CollectionMapping = FluentNHibernate.MappingModel.Collections.CollectionMapping;

namespace FluentNHibernate.Visitors
{
    public class ConventionVisitor : DefaultMappingModelVisitor
    {
        readonly Dictionary<Collection, Action<MappingModel.Collections.CollectionMapping>> collections;
        readonly IConventionFinder finder;
        Type currentType;

        public ConventionVisitor(IConventionFinder finder)
        {
            collections = new Dictionary<Collection, Action<MappingModel.Collections.CollectionMapping>>
            {
                { Collection.Array, ProcessArray },
                { Collection.Bag, ProcessBag },
                { Collection.Map, ProcessMap },
                { Collection.List, ProcessList },
                { Collection.Set, ProcessSet },
            };
            this.finder = finder;
        }

        public override void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            var conventions = finder.Find<IHibernateMappingConvention>();

            Apply<IHibernateMappingInspector, IHibernateMappingInstance>(conventions,
                new HibernateMappingInstance(hibernateMapping));
        }

        public override void ProcessId(IdMapping idMapping)
        {
            var conventions = finder.Find<IIdConvention>();

            Apply<IIdentityInspector, IIdentityInstance>(conventions,
                new IdentityInstance(idMapping));
        }

        public override void ProcessCompositeId(CompositeIdMapping idMapping)
        {
            var conventions = finder.Find<ICompositeIdentityConvention>();

            Apply<ICompositeIdentityInspector, ICompositeIdentityInstance>(conventions,
                new CompositeIdentityInstance(idMapping));
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            var conventions = finder.Find<IClassConvention>();

            currentType = classMapping.Type;

            Apply<IClassInspector, IClassInstance>(conventions,
                new ClassInstance(classMapping));
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            var conventions = finder.Find<IPropertyConvention>();

            Apply<IPropertyInspector, IPropertyInstance>(conventions,
                new PropertyInstance(propertyMapping));
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            var conventions = finder.Find<IColumnConvention>();

            Apply<IColumnInspector, IColumnInstance>(conventions,
                new ColumnInstance(currentType, columnMapping));
        }

        public override void ProcessCollection(CollectionMapping mapping)
        {
            var generalConventions = finder.Find<ICollectionConvention>();

            Apply<ICollectionInspector, ICollectionInstance>(generalConventions,
                new CollectionInstance(mapping));

            if (mapping.Relationship is ManyToManyMapping)
            {
                var conventions = finder.Find<IHasManyToManyConvention>();

                Apply<IManyToManyCollectionInspector, IManyToManyCollectionInstance>(conventions,
                    new ManyToManyCollectionInstance(mapping));
            }
            else
            {
                var conventions = finder.Find<IHasManyConvention>();

                Apply<IOneToManyCollectionInspector, IOneToManyCollectionInstance>(conventions,
                    new OneToManyCollectionInstance(mapping));
            }

            collections[mapping.Collection](mapping);
        }

#pragma warning disable 612,618
        void ProcessArray(CollectionMapping mapping)
        {
            var conventions = finder.Find<IArrayConvention>();

            Apply<IArrayInspector, IArrayInstance>(conventions,
                new CollectionInstance(mapping));
        }

        void ProcessBag(CollectionMapping mapping)
        {
            var conventions = finder.Find<IBagConvention>();

            Apply<IBagInspector, IBagInstance>(conventions,
                new CollectionInstance(mapping));
        }

        void ProcessList(CollectionMapping mapping)
        {
            var conventions = finder.Find<IListConvention>();

            Apply<IListInspector, IListInstance>(conventions,
                new CollectionInstance(mapping));
        }

        void ProcessMap(CollectionMapping mapping)
        {
            var conventions = finder.Find<IMapConvention>();

            Apply<IMapInspector, IMapInstance>(conventions,
                new CollectionInstance(mapping));
        }

        void ProcessSet(CollectionMapping mapping)
        {
            var conventions = finder.Find<ISetConvention>();

            Apply<ISetInspector, ISetInstance>(conventions,
                new CollectionInstance(mapping));
        }
#pragma warning restore 612,618

        public override void ProcessManyToOne(ManyToOneMapping mapping)
        {
            var conventions = finder.Find<IReferenceConvention>();

            Apply<IManyToOneInspector, IManyToOneInstance>(conventions,
                new ManyToOneInstance(mapping));
        }

        public override void ProcessVersion(VersionMapping mapping)
        {
            var conventions = finder.Find<IVersionConvention>();

            Apply<IVersionInspector, IVersionInstance>(conventions,
                new VersionInstance(mapping));
        }

        public override void ProcessOneToOne(OneToOneMapping mapping)
        {
            var conventions = finder.Find<IHasOneConvention>();

            Apply<IOneToOneInspector, IOneToOneInstance>(conventions,
                new OneToOneInstance(mapping));
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            if (subclassMapping.SubclassType == SubclassType.Subclass)
            {
                var conventions = finder.Find<ISubclassConvention>();

                Apply<ISubclassInspector, ISubclassInstance>(conventions,
                    new SubclassInstance(subclassMapping));
            }
            else
            {
                var conventions = finder.Find<IJoinedSubclassConvention>();

                Apply<IJoinedSubclassInspector, IJoinedSubclassInstance>(conventions,
                    new JoinedSubclassInstance(subclassMapping));
            }
        }

        public override void ProcessComponent(ComponentMapping mapping)
        {
            if (mapping.ComponentType == ComponentType.Component)
            {
                var conventions = finder.Find<IComponentConvention>();

                Apply<IComponentInspector, IComponentInstance>(conventions,
                    new ComponentInstance(mapping));
            }
            else
            {
                var conventions = finder.Find<IDynamicComponentConvention>();

                Apply<IDynamicComponentInspector, IDynamicComponentInstance>(conventions,
                    new DynamicComponentInstance(mapping));
            }
        }

        public override void ProcessIndex(IndexMapping indexMapping)
        {
            var conventions = finder.Find<IIndexConvention>();

            Apply<IIndexInspector, IIndexInstance>(conventions,
                new IndexInstance(indexMapping));
        }

        public override void ProcessIndex(IndexManyToManyMapping indexMapping)
        {
            var conventions = finder.Find<IIndexManyToManyConvention>();

            Apply<IIndexManyToManyInspector, IIndexManyToManyInstance>(conventions,
                new IndexManyToManyInstance(indexMapping));
        }

        public override void ProcessJoin(JoinMapping joinMapping)
        {
            var conventions = finder.Find<IJoinConvention>();

            Apply<IJoinInspector, IJoinInstance>(conventions,
                new JoinInstance(joinMapping));
        }

        public override void ProcessKeyProperty(KeyPropertyMapping mapping)
        {
            var conventions = finder.Find<IKeyPropertyConvention>();

            Apply<IKeyPropertyInspector, IKeyPropertyInstance>(conventions, 
                new KeyPropertyInstance(mapping));
        }

        public override void ProcessKeyManyToOne(KeyManyToOneMapping mapping)
        {
            var conventions = finder.Find<IKeyManyToOneConvention>();

            Apply<IKeyManyToOneInspector, IKeyManyToOneInstance>(conventions, 
                new KeyManyToOneInstance(mapping));
        }

        public override void ProcessAny(AnyMapping mapping)
        {
            var conventions = finder.Find<IAnyConvention>();

            Apply<IAnyInspector, IAnyInstance>(conventions,
                new AnyInstance(mapping));
        }

        static void Apply<TInspector, TInstance>(IEnumerable conventions, TInstance instance)
            where TInspector : IInspector
            where TInstance : TInspector
        {
            foreach (IConvention<TInspector, TInstance> convention in conventions)
            {
                var criteria = new ConcreteAcceptanceCriteria<TInspector>();
                var acceptance = convention as IConventionAcceptance<TInspector>;

                if (acceptance != null)
                    acceptance.Accept(criteria);

                if (criteria.Matches(instance))
                    convention.Apply(instance);
            }
        }
    }
}