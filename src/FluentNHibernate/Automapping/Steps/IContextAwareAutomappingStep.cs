using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps;

/// <summary>
/// An automapping step whose decision to map a member can depend on what has already
/// been mapped on the class, not just on the member in isolation.
/// </summary>
public interface IContextAwareAutomappingStep
{
    bool ShouldMap(ClassMappingBase classMap, Member member);
}
