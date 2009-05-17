using System;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class UnmappedPropertyException : Exception
    {
        public UnmappedPropertyException(Type type, string name)
            : base("Unmapped property '" + name + "' on type '" + type.Name + "'")
        {}
    }
}