using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class Cache
    {
        public static IClassConvention Is(Action<ICache> cache)
        {
            return new BuiltClassConvention(
                map => !map.Cache.IsDirty,
                map => cache(map.Cache));
        }
    }
}