using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

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

            if (member.IsProperty && !member.CanWrite)
                mapping.SetDefaultValue(x => x.Access, cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());

            return mapping;
        }
    }
}
