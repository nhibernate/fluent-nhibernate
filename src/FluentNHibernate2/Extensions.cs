using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate
{
    public static class Extensions
    {
        public static void Each<T> (this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");
            if (action == null) throw new ArgumentNullException("action");

            foreach (T t in enumerable)
                action(t);
        }
    }
}
