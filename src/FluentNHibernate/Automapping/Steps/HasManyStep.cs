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

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (member.DeclaringType != classMap.Type)
                return;

            if (simpleTypeCollectionStepStep.ShouldMap(member))
                simpleTypeCollectionStepStep.Map(classMap, member);
            else if (collectionStep.ShouldMap(member))
                collectionStep.Map(classMap, member);
        }
    }
}
