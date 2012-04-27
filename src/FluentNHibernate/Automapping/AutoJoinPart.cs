using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
    public class AutoJoinPart<T> : JoinPart<T>
    {
        readonly IList<Member> mappedMembers;

        public AutoJoinPart(IList<Member> mappedMembers, string tableName)
            : base(tableName)
        {
            this.mappedMembers = mappedMembers;
        }

        internal override void OnMemberMapped(Member member)
        {
            mappedMembers.Add(member);
        }
    }
}