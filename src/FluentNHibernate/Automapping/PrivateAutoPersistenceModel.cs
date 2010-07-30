using System;

namespace FluentNHibernate.Automapping
{
    [Obsolete("Depreciated in favour of supplying your own IAutomappingConfiguration instance to AutoMap: AutoMap.AssemblyOf<T>(your_configuration_instance)")]
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            Setup(s =>
            {
                s.FindIdentity = findIdentity;
                s.FindMembers = findMembers;
            });
        }

        static bool findMembers(Member member)
        {
            return member.IsField && member.IsPrivate;
        }

        static bool findIdentity(Member member)
        {
            return member.IsField && member.IsPrivate && member.Name == "id";
        }
    }
}