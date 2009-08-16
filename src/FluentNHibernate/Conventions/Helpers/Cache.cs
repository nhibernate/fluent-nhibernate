using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class Cache
    {
        public static IClassConvention Is(Action<ICacheInstance> cache)
        {
            return new BuiltClassConvention(
                criteria => criteria.Expect(x => x.Cache, AcceptanceCriteria.Is.Not.Set),
                instance => cache(instance.Cache));
        }
    }
}