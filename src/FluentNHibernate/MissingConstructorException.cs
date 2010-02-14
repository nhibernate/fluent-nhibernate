using System;
using System.Runtime.Serialization;

namespace FluentNHibernate
{
    [Serializable]
    public class MissingConstructorException : Exception
    {
        public MissingConstructorException(Type type)
            : base("'" + type.AssemblyQualifiedName + "' is missing a parameterless constructor.")
        {}

        protected MissingConstructorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}