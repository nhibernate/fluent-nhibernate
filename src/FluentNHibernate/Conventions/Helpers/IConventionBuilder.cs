using System;

namespace FluentNHibernate.Conventions.Helpers
{
    public interface IConventionBuilder<TConvention, TConventionTarget>
        where TConvention : IConvention<TConventionTarget>
    {
        TConvention Always(Action<TConventionTarget> convention);
        TConvention When(Func<TConventionTarget, bool> isTrue, Action<TConventionTarget> convention);
    }
}