using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps;

public class HasManyStep(IAutomappingConfiguration cfg) : IAutomappingStep
{
    readonly SimpleTypeCollectionStep simpleTypeCollectionStepStep = new(cfg);
    readonly CollectionStep collectionStep = new(cfg);

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
