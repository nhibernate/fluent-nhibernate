using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class ConventionBuilder
    {
        public static IConventionBuilder<IClassConvention, IClassInspector, IClassAlteration> Class
        {
            get { return new ClassConventionBuilder(); }
        }

        public static IConventionBuilder<IIdConvention, IIdentityInspector, IIdentityAlteration> Id
        {
            get { return new IdConventionBuilder(); }
        }

        public static IConventionBuilder<IPropertyConvention, IPropertyInspector, IPropertyAlteration> Property
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

        public static IConventionBuilder<IHasManyConvention, IOneToManyCollectionInspector, IOneToManyCollectionAlteration> HasMany
        {
            get { return new HasManyConventionBuilder(); }
        }

        public static IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionAlteration> HasManyToMany
        {
            get { return new HasManyToManyConventionBuilder(); }
        }

        //public static IConventionBuilder<IHasOneConvention, IOneToOnePart> HasOne
        //{
        //    get { return new HasOneConventionBuilder(); }
        //}

        public static IConventionBuilder<IReferenceConvention, IManyToOneInspector, IManyToOneAlteration> Reference
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