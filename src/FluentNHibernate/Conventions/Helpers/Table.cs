using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class Table
    {
        public static IClassConvention Is(Func<IClassInspector, string> tableName)
        {
            return new BuiltClassConvention(
                acceptance => { },
                (alteration, inspector) =>
                {
                    var table = tableName(inspector);
                    alteration.WithTable(table);
                });
        }
    }
}