using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class OptimisticLock
    {
        public static IClassConvention Is(Action<IOptimisticLockInstance> locking)
        {
            return new BuiltClassConvention(
                criteria => criteria.Expect(x => x.OptimisticLock, AcceptanceCriteria.Is.Not.Set), // eww
                x => locking(x.OptimisticLock));
        }
    }
}