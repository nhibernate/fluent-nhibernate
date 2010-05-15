using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public class ComponentStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;
        private readonly AutoMapper mapper;

        public ComponentStep(IAutomappingConfiguration cfg, AutoMapper mapper)
        {
            this.cfg = cfg;
            this.mapper = mapper;
        }

        public bool ShouldMap(Member member)
        {
            return cfg.IsComponent(member.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            var mapping = new ComponentMapping(ComponentType.Component)
            {
                Name = member.Name,
                Member = member,
                ContainingEntityType = classMap.Type,
                Type = member.PropertyType
            };

            mapper.FlagAsMapped(member.PropertyType);
            mapper.MergeMap(member.PropertyType, mapping, new List<Member>());

            classMap.AddComponent(mapping);
        }
    }
}