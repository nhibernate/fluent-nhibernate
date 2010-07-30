using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class DefaultAutomappingConfiguration : IAutomappingConfiguration
    {
        public virtual bool ShouldMap(Member member)
        {
            return member.IsProperty && member.IsPublic;
        }

        public virtual bool ShouldMap(Type type)
        {
            return !type.ClosesInterface(typeof(IAutoMappingOverride<>)) &&
                !type.HasInterface(typeof(IMappingProvider)) &&
                !type.IsNested;
        }

        public virtual bool IsId(Member member)
        {
            return member.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
        }

        public virtual Access GetAccessStrategyForReadOnlyProperty(Member member)
        {
            if (member.IsAutoProperty)
                return Access.BackField;

            return Access.ReadOnlyPropertyThroughCamelCaseField();
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
            return false;
        }

        public virtual string GetDiscriminatorColumn(Type type)
        {
            return "discriminator";
        }

        [Obsolete("Use IsDiscriminated instead.", true)]
        public SubclassStrategy GetSubclassStrategy(Type type)
        {
            throw new NotSupportedException();
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
                new VersionStep(this),
                new ComponentStep(this, mapper),
                new PropertyStep(conventionFinder, this),
                new HasManyToManyStep(this),
                new ReferenceStep(this),
                new HasManyStep(this)
            };
        }
    }
}