using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DynamicUpdate
    {
        public static IClassConvention AlwaysTrue()
        {
            return new BuiltClassConvention(
                criteria => criteria.Expect(x => x.DynamicUpdate, Is.Not.Set),
                x => x.DynamicUpdate());
        }

        public static IClassConvention AlwaysFalse()
        {
            return new BuiltClassConvention(
                criteria => criteria.Expect(x => x.DynamicUpdate, Is.Not.Set),
                x => x.Not.DynamicUpdate());
        }
    }
}