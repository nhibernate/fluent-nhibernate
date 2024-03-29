using System;

namespace FluentNHibernate.Visitors;

[Serializable]
public class UnresolvedComponentReferenceVisitedException(
    Type referencedComponentType,
    Type sourceType,
    Member sourceMember)
    : Exception(string.Format(
        "Visitor attempted on unresolved componented reference '{0}', referenced from property '{1}' of '{2}', unable to continue.",
        referencedComponentType.Name, sourceMember.Name, sourceType.Name))
{
    public Type ReferencedComponentType { get; } = referencedComponentType;
    public Type SourceType { get; } = sourceType;
    public Member SourceMember { get; } = sourceMember;
}
