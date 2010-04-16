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

        public void Map(ClassMappingBase classMap, Member property)
        {
            var mapping = new ComponentMapping(ComponentType.Component)
            {
                Name = property.Name,
                Member = property,
                ContainingEntityType = classMap.Type,
                Type = property.PropertyType
            };

            mapper.FlagAsMapped(property.PropertyType);
            mapper.MergeMap(property.PropertyType, mapping, new List<Member>());

            classMap.AddComponent(mapping);
        }
    }
}