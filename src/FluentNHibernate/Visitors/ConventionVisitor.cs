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

namespace FluentNHibernate.Visitors;

public class ConventionVisitor : DefaultMappingModelVisitor
{
    readonly Dictionary<Collection, Action<CollectionMapping>> collections;
    readonly IConventionFinder finder;
    Type currentType;

    public ConventionVisitor(IConventionFinder finder)
    {
        collections = new Dictionary<Collection, Action<CollectionMapping>>
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

        Apply(conventions, new HibernateMappingInstance(hibernateMapping));
    }

    public override void ProcessId(IdMapping idMapping)
    {
        var conventions = finder.Find<IIdConvention>();

        Apply(conventions, new IdentityInstance(idMapping));
    }

    public override void ProcessCompositeId(CompositeIdMapping idMapping)
    {
        var conventions = finder.Find<ICompositeIdentityConvention>();

        Apply(conventions, new CompositeIdentityInstance(idMapping));
    }

    public override void ProcessClass(ClassMapping classMapping)
    {
        var conventions = finder.Find<IClassConvention>();

        currentType = classMapping.Type;

        Apply(conventions, new ClassInstance(classMapping));
    }

    public override void ProcessProperty(PropertyMapping propertyMapping)
    {
        var conventions = finder.Find<IPropertyConvention>();

        Apply(conventions, new PropertyInstance(propertyMapping));
    }

    public override void ProcessColumn(ColumnMapping columnMapping)
    {
        var conventions = finder.Find<IColumnConvention>();

        Apply(conventions, new ColumnInstance(currentType, columnMapping));
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
        var generalConventions = finder.Find<ICollectionConvention>();

        Apply(generalConventions, new CollectionInstance(mapping));

        if (mapping.Relationship is ManyToManyMapping)
        {
            var conventions = finder.Find<IHasManyToManyConvention>();

            Apply(conventions, new ManyToManyCollectionInstance(mapping));
        }
        else
        {
            var conventions = finder.Find<IHasManyConvention>();

            Apply(conventions, new OneToManyCollectionInstance(mapping));
        }

        if (collections.TryGetValue(mapping.Collection, out var processor))
        {
            processor(mapping);
        }
    }

#pragma warning disable 612,618
    void ProcessArray(CollectionMapping mapping)
    {
        var conventions = finder.Find<IArrayConvention>();

        Apply(conventions, new CollectionInstance(mapping));
    }

    void ProcessBag(CollectionMapping mapping)
    {
        var conventions = finder.Find<IBagConvention>();

        Apply(conventions, new CollectionInstance(mapping));
    }

    void ProcessList(CollectionMapping mapping)
    {
        var conventions = finder.Find<IListConvention>();

        Apply(conventions, new CollectionInstance(mapping));
    }

    void ProcessMap(CollectionMapping mapping)
    {
        var conventions = finder.Find<IMapConvention>();

        Apply(conventions, new CollectionInstance(mapping));
    }

    void ProcessSet(CollectionMapping mapping)
    {
        var conventions = finder.Find<ISetConvention>();

        Apply(conventions, new CollectionInstance(mapping));
    }
#pragma warning restore 612,618

    public override void ProcessManyToOne(ManyToOneMapping mapping)
    {
        var conventions = finder.Find<IReferenceConvention>();

        Apply(conventions, new ManyToOneInstance(mapping));
    }

    public override void ProcessVersion(VersionMapping mapping)
    {
        var conventions = finder.Find<IVersionConvention>();

        Apply(conventions, new VersionInstance(mapping));
    }

    public override void ProcessOneToOne(OneToOneMapping mapping)
    {
        var conventions = finder.Find<IHasOneConvention>();

        Apply(conventions, new OneToOneInstance(mapping));
    }

    public override void ProcessSubclass(SubclassMapping subclassMapping)
    {
        if (subclassMapping.SubclassType == SubclassType.Subclass)
        {
            var conventions = finder.Find<ISubclassConvention>();

            Apply(conventions, new SubclassInstance(subclassMapping));
        }
        else
        {
            var conventions = finder.Find<IJoinedSubclassConvention>();

            Apply(conventions, new JoinedSubclassInstance(subclassMapping));
        }
    }

    public override void ProcessComponent(ComponentMapping mapping)
    {
        if (mapping.ComponentType == ComponentType.Component)
        {
            var conventions = finder.Find<IComponentConvention>();

            Apply(conventions, new ComponentInstance(mapping));
        }
        else
        {
            var conventions = finder.Find<IDynamicComponentConvention>();

            Apply(conventions, new DynamicComponentInstance(mapping));
        }
    }

    public override void ProcessIndex(IndexMapping indexMapping)
    {
        var conventions = finder.Find<IIndexConvention>();

        Apply(conventions, new IndexInstance(indexMapping));
    }

    public override void ProcessIndex(IndexManyToManyMapping indexMapping)
    {
        var conventions = finder.Find<IIndexManyToManyConvention>();

        Apply(conventions, new IndexManyToManyInstance(indexMapping));
    }

    public override void ProcessJoin(JoinMapping joinMapping)
    {
        var conventions = finder.Find<IJoinConvention>();

        Apply(conventions, new JoinInstance(joinMapping));
    }

    public override void ProcessKeyProperty(KeyPropertyMapping mapping)
    {
        var conventions = finder.Find<IKeyPropertyConvention>();

        Apply(conventions, new KeyPropertyInstance(mapping));
    }

    public override void ProcessKeyManyToOne(KeyManyToOneMapping mapping)
    {
        var conventions = finder.Find<IKeyManyToOneConvention>();

        Apply(conventions, new KeyManyToOneInstance(mapping));
    }

    public override void ProcessAny(AnyMapping mapping)
    {
        var conventions = finder.Find<IAnyConvention>();

        Apply(conventions, new AnyInstance(mapping));
    }

    static void Apply<TInspector, TInstance>(IEnumerable<IConvention<TInspector, TInstance>> conventions, TInstance instance)
        where TInspector : IInspector
        where TInstance : class, TInspector
    {
        foreach (var convention in conventions)
        {
            var criteria = new ConcreteAcceptanceCriteria<TInspector>();
            var acceptance = convention as IConventionAcceptance<TInspector>;

            acceptance?.Accept(criteria);

            if (criteria.Matches(instance))
                convention.Apply(instance);
        }
    }
}
