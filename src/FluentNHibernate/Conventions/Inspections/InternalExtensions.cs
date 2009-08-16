using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    internal static class InternalExtensions
    {
        public static IDefaultableList<T> ToDefaultableList<T>(this IEnumerable<T> enumerable)
        {
            return new DefaultableList<T>(enumerable);
        }
    }
}