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

            mapping.Set(x => x.Name, Layer.Defaults, member.Name);
            mapping.Set(x => x.Class, Layer.Defaults, new TypeReference(member.PropertyType));
            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, member.Name + "_id");
            mapping.AddColumn(Layer.Defaults, columnMapping);

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
                mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
                mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
        }

    }
}
