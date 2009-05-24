using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class OptimisticLock
    {
        public static IClassConvention Is(Action<IOptimisticLockBuilder> locking)
        {
            return new BuiltClassConvention(
                map => !map.Attributes.Has("optimistic-lock"),
                map => locking(map.OptimisticLock));
        }
    }
}