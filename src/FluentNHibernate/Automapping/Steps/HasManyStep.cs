using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public class HasManyStep : IAutomappingStep
    {
        readonly SimpleTypeCollectionStep simpleTypeCollectionStepStep;
        readonly CollectionStep collectionStep;

        public HasManyStep(IAutomappingConfiguration cfg)
        {
            simpleTypeCollectionStepStep = new SimpleTypeCollectionStep(cfg);
            collectionStep = new CollectionStep(cfg);
        }

        public bool ShouldMap(Member member)
        {
            return simpleTypeCollectionStepStep.ShouldMap(member) ||
                   collectionStep.ShouldMap(member);
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            if (simpleTypeCollectionStepStep.ShouldMap(property))
                simpleTypeCollectionStepStep.Map(classMap, property);
            else if (collectionStep.ShouldMap(property))
                collectionStep.Map(classMap, property);
        }
    }
}
