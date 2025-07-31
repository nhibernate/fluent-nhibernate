using System;

namespace FluentNHibernate.Visitors;

[Serializable]
public class MissingExternalComponentException(Type referencedComponentType, Type sourceType, Member sourceMember)
    : Exception(string.Format("Unable to find external component for '{0}', referenced from property '{1}' of '{2}'.",
        referencedComponentType.Name, sourceMember.Name, sourceType.Name))
{
    public Type ReferencedComponentType { get; } = referencedComponentType;
    public Type SourceType { get; } = sourceType;
    public Member SourceMember { get; } = sourceMember;
}
