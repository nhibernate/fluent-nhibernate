using System;

namespace FluentNHibernate.Visitors;

[Serializable]
public class UnresolvedComponentReferenceVisitedException : Exception
{
    public UnresolvedComponentReferenceVisitedException(Type referencedComponentType, Type sourceType, Member sourceMember)
        : base(string.Format("Visitor attempted on unresolved componented reference '{0}', referenced from property '{1}' of '{2}', unable to continue.",
            referencedComponentType.Name, sourceMember.Name, sourceType.Name))
    {
        ReferencedComponentType = referencedComponentType;
        SourceType = sourceType;
        SourceMember = sourceMember;
    }

    public Type ReferencedComponentType { get; }
    public Type SourceType { get; }
    public Member SourceMember { get; }
}
