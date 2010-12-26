using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public class ComponentStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;

        public ComponentStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            return cfg.IsComponent(member.PropertyType);
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            // don't map the component here, mark it as a reference which'll
            // allow us to integrate with ComponentMap or automap at a later
            // stage
            var mapping = new ReferenceComponentMapping(ComponentType.Component, member, member.PropertyType, classMap.Type, cfg.GetComponentColumnPrefix(member));

            classMap.AddComponent(mapping);
        }
    }
}