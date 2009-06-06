using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DynamicUpdate
    {
        public static IClassConvention AlwaysTrue()
        {
            throw new NotImplementedException("Awaiting conventions DSL");
        }

        public static IClassConvention AlwaysFalse()
        {
            throw new NotImplementedException("Awaiting conventions DSL");
        }
    }
}