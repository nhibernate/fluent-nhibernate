using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DynamicInsert
    {
        public static IClassConvention AlwaysTrue()
        {
            return new BuiltClassConvention(
                criteria => criteria.Expect(x => x.DynamicInsert, Is.Not.Set),
                x => x.DynamicInsert());
        }

        public static IClassConvention AlwaysFalse()
        {
            return new BuiltClassConvention(
                criteria => criteria.Expect(x => x.DynamicInsert, Is.Not.Set),
                x => x.Not.DynamicInsert());
        }
    }
}