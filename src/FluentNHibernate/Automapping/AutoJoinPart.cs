#if USE_NULLABLE
#nullable enable
#endif
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping;

public class AutoJoinPart<T>(IList<Member> mappedMembers, string tableName) : JoinPart<T>(tableName)
{
    internal override void OnMemberMapped(Member member)
    {
        mappedMembers.Add(member);
    }
}
