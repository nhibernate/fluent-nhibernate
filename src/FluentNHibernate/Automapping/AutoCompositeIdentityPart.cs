using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
    public class AutoCompositeIdentityPart<T> : CompositeIdentityPart<T>
    {
        private readonly IList<Member> mappedMembers;

        public AutoCompositeIdentityPart(IList<Member> mappedMembers)
        {
            this.mappedMembers = mappedMembers;
        }

        protected override CompositeIdentityPart<T> KeyProperty(Member member, string columnName, Action<KeyPropertyPart> customMapping)
        {
            mappedMembers.Add(member);

            return base.KeyProperty(member, columnName, customMapping);
        }

        protected override CompositeIdentityPart<T> KeyReference(Member property, IEnumerable<string> columnNames, Action<KeyManyToOnePart> customMapping)
        {
            mappedMembers.Add(property);

            return base.KeyReference(property, columnNames, customMapping);
        }
    }
}
