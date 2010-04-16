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

        protected override CompositeIdentityPart<T> KeyProperty(Member property, string columnName, Action<KeyPropertyPart> customMapping)
        {
            mappedMembers.Add(property);

            return base.KeyProperty(property, columnName, customMapping);
        }

        protected override CompositeIdentityPart<T> KeyReference(Member property, string columnName, Action<KeyManyToOnePart> customMapping)
        {
            mappedMembers.Add(property);

            return base.KeyReference(property, columnName, customMapping);
        }
    }
}
