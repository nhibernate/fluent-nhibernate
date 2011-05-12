using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

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
            return member.PropertyType.Name.In("ISet", "ISet`1", "HashSet`1");
        }

        static bool IsEnumerable(Member member)
        {
            return member.PropertyType == typeof(IEnumerable) || member.PropertyType.Closes(typeof(IEnumerable<>));
        }
    }
}