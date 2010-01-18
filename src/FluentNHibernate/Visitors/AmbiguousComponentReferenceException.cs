using System;

namespace FluentNHibernate.Visitors
{
    [Serializable]
    public class AmbiguousComponentReferenceException : Exception
    {
        public AmbiguousComponentReferenceException(Type referencedComponentType, Type sourceType, Member sourceMember)
            : base(string.Format("Multiple external components for '{0}', referenced from property '{1}' of '{2}', unable to continue.",
                referencedComponentType.Name, sourceMember.Name, sourceType.Name))
        {
            ReferencedComponentType = referencedComponentType;
            SourceType = sourceType;
            SourceMember = sourceMember;
        }

        public Type ReferencedComponentType { get; private set; }
        public Type SourceType { get; private set; }
        public Member SourceMember { get; private set; }
    }
}