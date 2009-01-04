using System;

namespace FluentNHibernate
{
    public class MissingConstructorException : Exception
    {
        public MissingConstructorException(Type type)
            : base("'" + type.AssemblyQualifiedName + "' is missing a parameterless constructor.")
        {}
    }
}