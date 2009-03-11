using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
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