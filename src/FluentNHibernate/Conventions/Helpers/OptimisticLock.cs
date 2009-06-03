using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class OptimisticLock
    {
        public static IClassConvention Is(Action<IOptimisticLockBuilder> locking)
        {
            throw new NotImplementedException("Awaiting conventions DSL");
        }
    }
}