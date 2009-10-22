using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Infrastructure
{
    [Serializable]
    public class ResolveException : Exception
    {
        public ResolveException(Type type)
            : base("Unable to resolve dependency: '" + type.FullName + "'")
        {}

        protected ResolveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}