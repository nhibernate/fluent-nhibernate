using System;

namespace FluentNHibernate.Infrastructure
{
    public class ResolveException : Exception
    {
        public ResolveException(Type type)
            : base("Unable to resolve dependency: '" + type.FullName + "'")
        {}
    }
}