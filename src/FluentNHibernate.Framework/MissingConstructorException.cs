using System;

namespace FluentNHibernate.Framework
{
    public class MissingConstructorException : Exception
    {
        public MissingConstructorException(Type type)
            : base("'" + type.Name + "' is missing a parameterless constructor.")
        {}
    }
}