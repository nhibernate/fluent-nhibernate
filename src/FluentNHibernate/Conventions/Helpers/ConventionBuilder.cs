using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class ConventionBuilder
    {
        public static IConventionBuilder<IClassConvention, IClassInspector, IClassInstance> Class
        {
            get { return new ClassConventionBuilder(); }
        }

        public static IConventionBuilder<IIdConvention, IIdentityInspector, IIdentityInstance> Id
        {
            get { return new IdConventionBuilder(); }
        }

        public static IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyInstance> Property
        {
            get { return new PropertyConventionBuilder(); }
        }

        //public static IConventionBuilder<IVersionConvention, IVersion> Version
        //{
        //    get { return new VersionConventionBuilder(); }
        //}

        //public static IConventionBuilder<IComponentConvention, IComponent> Component
        //{
        //    get { return new ComponentConventionBuilder(); }
        //}

        //public static IConventionBuilder<IDynamicComponentConvention, IDynamicComponent> DynamicComponent
        //{
        //    get { return new DynamicComponentConventionBuilder(); }
        //}

        public static IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionInstance> HasMany
        {
            get { return new HasManyConventionBuilder(); }
        }

        public static IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionInstance> HasManyToMany
        {
            get { return new HasManyToManyConventionBuilder(); }
        }

        //public static IConventionBuilder<IArrayConvention, IArrayInspector, IArrayInstance> AsArray
        //{
        //    get { return new ArrayConventionBuilder(); }
        //}

        //public static IConventionBuilder<IBagConvention, IBagInspector, IBagInstance> AsBag
        //{
        //    get { return new BagConventionBuilder(); }
        //}

        //public static IConventionBuilder<ISetConvention, ISetInspector, ISetInstance> AsSet
        //{
        //    get { return new SetConventionBuilder(); }
        //}

        //public static IConventionBuilder<IListConvention, IListInspector, IListInstance> AsList
        //{
        //    get { return new ListConventionBuilder(); }
        //}

        //public static IConventionBuilder<IMapConvention, IMapInspector, IMapInstance> AsMap
        //{
        //    get { return new MapConventionBuilder(); }
        //}

        //public static IConventionBuilder<IHasOneConvention, IOneToOnePart> HasOne
        //{
        //    get { return new HasOneConventionBuilder(); }
        //}

        public static IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneInstance> Reference
        {
            get { return new ReferenceConventionBuilder(); }
        }

        //public static IConventionBuilder<IRelationshipConvention, IRelationship> Relationship
        //{
        //    get { return new RelationshipConventionBuilder(); }
        //}

        //public static IConventionBuilder<IJoinConvention, IJoin> Join
        //{
        //    get { return new JoinConventionBuilder(); }
        //}
    }
}