using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class Table
    {
        public static IClassConvention Is(Func<IClassMap, string> tableName)
        {
            return new BuiltClassConvention(
                map => true,
                map =>
                {
                    var table = tableName(map);
                    map.WithTable(table);
                });
        }
    }
}