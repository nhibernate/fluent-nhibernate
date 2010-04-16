using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping
{
    public class DefaultAutomappingConfiguration : IAutomappingConfiguration
    {
        public virtual bool ShouldMap(Member member)
        {
            return member.IsProperty && member.CanWrite;
        }

        public virtual bool ShouldMap(Type type)
        {
            return true;
        }

        public virtual bool IsId(Member member)
        {
            return member.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
        }

        public virtual Type GetParentSideForManyToMany(Type left, Type right)
        {
            return left.FullName.CompareTo(right.FullName) < 0 ? left : right;
        }

        public virtual bool IsConcreteBaseType(Type type)
        {
            return false;
        }

        public virtual bool IsComponent(Type type)
        {
            return false;
        }

        public virtual string GetComponentColumnPrefix(Member member)
        {
            return member.Name;
        }

        public virtual bool IsDiscriminated(Type type)
        {
            return GetSubclassStrategy(type) == SubclassStrategy.Subclass;
        }

        public virtual string GetDiscriminatorColumn(Type type)
        {
            return "discriminator";
        }

        public virtual SubclassStrategy GetSubclassStrategy(Type type)
        {
            return SubclassStrategy.JoinedSubclass;
        }

        public virtual bool AbstractClassIsLayerSupertype(Type type)
        {
            return true;
        }

        public virtual string SimpleTypeCollectionValueColumn(Member member)
        {
            return "Value";
        }

        public virtual IEnumerable<IAutomappingStep> GetMappingSteps(AutoMapper mapper, IConventionFinder conventionFinder)
        {
            return new IAutomappingStep[]
            {
                new IdentityStep(this),
                new VersionStep(),
                new ComponentStep(this, mapper),
                new PropertyStep(conventionFinder, this),
                new HasManyToManyStep(this),
                new ReferenceStep(),
                new HasManyStep(this)
            };
        }
    }
}