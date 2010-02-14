using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel
{
    public static class EqualityExtensions
    {
        public static bool ContentEquals<T>(this IDefaultableEnumerable<T> left, IDefaultableEnumerable<T> right)
        {
            return left.Defaults.ContentEquals(right.Defaults) && left.UserDefined.ContentEquals(right.UserDefined);
        }

        public static bool ContentEquals<TKey, TValue>(this IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
        {
            if (left.Count() != right.Count())
                return false;

            var index = 0;
            foreach (var item in left)
            {
                if (!item.Equals(right.ElementAt(index)))
                    return false;

                index++;
            }

            return true;
        }

        public static bool ContentEquals<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            if (left.Count() != right.Count())
                return false;

            var index = 0;
            foreach (var item in left)
            {
                if (!item.Equals(right.ElementAt(index)))
                    return false;

                index++;
            }

            return true;
        }
    }
}