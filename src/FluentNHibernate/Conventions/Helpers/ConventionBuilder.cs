using System.Collections.Generic;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class ConventionBuilder
    {
        public static IConventionBuilder<IEntireMappingsConvention, IEnumerable<IClassMap>> Everything
        {
            get { return new EverythingConventionBuilder(); }
        }

        public static IConventionBuilder<IClassConvention, IClassMap> Class
        {
            get { return new ClassConventionBuilder(); }
        }

        public static IConventionBuilder<IMappingPartConvention, IMappingPart> MappingPart
        {
            get { return new MappingPartConventionBuilder(); }
        }

        public static IConventionBuilder<IIdConvention, IIdentityPart> Id
        {
            get { return new IdConventionBuilder(); }
        }

        public static IConventionBuilder<IPropertyConvention, IProperty> Property
        {
            get { return new PropertyConventionBuilder(); }
        }

        public static IConventionBuilder<IVersionConvention, IVersion> Version
        {
            get { return new VersionConventionBuilder(); }
        }

        public static IConventionBuilder<IComponentConvention, IComponent> Component
        {
            get { return new ComponentConventionBuilder(); }
        }

        public static IConventionBuilder<IDynamicComponentConvention, IDynamicComponent> DynamicComponent
        {
            get { return new DynamicComponentConventionBuilder(); }
        }

        public static IConventionBuilder<IHasManyConvention, IOneToManyPart> HasMany
        {
            get { return new HasManyConventionBuilder(); }
        }

        public static IConventionBuilder<IHasManyToManyConvention, IManyToManyPart> HasManyToMany
        {
            get { return new HasManyToManyConventionBuilder(); }
        }

        public static IConventionBuilder<IHasOneConvention, IOneToOnePart> HasOne
        {
            get { return new HasOneConventionBuilder(); }
        }

        public static IConventionBuilder<IReferenceConvention, IManyToOnePart> Reference
        {
            get { return new ReferenceConventionBuilder(); }
        }

        public static IConventionBuilder<IRelationshipConvention, IRelationship> Relationship
        {
            get { return new RelationshipConventionBuilder(); }
        }

        public static IConventionBuilder<IJoinConvention, IJoin> Join
        {
            get { return new JoinConventionBuilder(); }
        }
    }
}