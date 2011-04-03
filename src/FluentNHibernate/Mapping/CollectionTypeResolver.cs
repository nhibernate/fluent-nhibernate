using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public static class CollectionTypeResolver
    {
        public static Collection Resolve(Member member)
        {
            Member backingField;

            if (IsEnumerable(member) && member.TryGetBackingField(out backingField))
                return Resolve(backingField);

            if (IsSet(member))
                return Collection.Set;

            return Collection.Bag;
        }

        static bool IsSet(Member member)
        {
            return member.PropertyType == typeof(ISet) || member.PropertyType.Closes(typeof(ISet<>)) || member.PropertyType.Closes(typeof(HashSet<>));
        }

        static bool IsEnumerable(Member member)
        {
            return member.PropertyType == typeof(IEnumerable) || member.PropertyType.Closes(typeof(IEnumerable<>));
        }
    }
}