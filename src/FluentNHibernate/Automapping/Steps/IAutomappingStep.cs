#if USE_NULLABLE
#nullable enable
#endif
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps;

public interface IAutomappingStep
{
    bool ShouldMap(Member member);
    void Map(ClassMappingBase classMap, Member member);
}
