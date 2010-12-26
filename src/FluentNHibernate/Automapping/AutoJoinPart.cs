using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
#pragma warning disable 612,618,672
    public class AutoJoinPart<T> : JoinPart<T>
    {
        readonly IList<Member> mappedMembers;

        public AutoJoinPart(IList<Member> mappedMembers, string tableName)
            : base(tableName)
        {
            this.mappedMembers = mappedMembers;
        }

        protected override ComponentPart<TComponent> Component<TComponent>(Member property, Action<ComponentPart<TComponent>> action)
        {
            mappedMembers.Add(property);

            return base.Component(property, action);
        }

        protected override OneToManyPart<TChild> HasMany<TChild>(Member member)
        {
            mappedMembers.Add(member);
            return base.HasMany<TChild>(member);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasManyToMany<TChild>(property);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(Member property)
        {
            mappedMembers.Add(property);
            return base.HasOne<TOther>(property);
        }

        protected override PropertyPart Map(Member property, string columnName)
        {
            mappedMembers.Add(property);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
        {
            mappedMembers.Add(property);
            return base.References<TOther>(property, columnName);
        }

        protected override AnyPart<TOther> ReferencesAny<TOther>(Member property)
        {
            mappedMembers.Add(property);
            return base.ReferencesAny<TOther>(property);
        }
    }
#pragma warning restore 612,618,672
}