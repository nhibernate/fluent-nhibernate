using System;

namespace FluentNHibernate
{
    public class UnknownPropertyException : Exception
    {
        public UnknownPropertyException(Type classType, string propertyName)
        {
            Type = classType;
            Property = propertyName;
        }

        public string Property { get; private set; }
        public Type Type { get; private set; }
    }
}