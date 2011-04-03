using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Automapping.Steps
{
    public class ReferenceStep : IAutomappingStep
    {
        private readonly Func<Member, bool> findPropertyconvention = p => (
            p.PropertyType.Namespace != "System" && // ignore clr types (won't be entities)
            p.PropertyType.Namespace != "System.Collections.Generic" &&
            p.PropertyType.Namespace != "Iesi.Collections.Generic" &&
	    !p.PropertyType.IsEnum);
        readonly IAutomappingConfiguration cfg;

        public ReferenceStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            return findPropertyconvention(member);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            var manyToOne = CreateMapping(member);
            manyToOne.ContainingEntityType = classMap.Type;
            classMap.AddReference(manyToOne);
        }

        private ManyToOneMapping CreateMapping(Member member)
        {
            var mapping = new ManyToOneMapping { Member = member };

            mapping.SetDefaultValue(x => x.Name, member.Name);
            mapping.SetDefaultValue(x => x.Class, new TypeReference(member.PropertyType));
            mapping.AddDefaultColumn(new ColumnMapping { Name = member.Name + "_id" });

            SetDefaultAccess(member, mapping);

            return mapping;
        }

        void SetDefaultAccess(Member member, ManyToOneMapping mapping)
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
            {
                // if it's a property or unset then we'll just let NH deal with it, otherwise
                // set the access to be whatever we determined it might be
                mapping.SetDefaultValue(x => x.Access, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.SetDefaultValue(x => x.Access, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
        }

    }
}
