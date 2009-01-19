using System;

namespace FluentNHibernate.AutoMap
{
    public class AutoMappingException : Exception
    {
        public AutoMappingException(string message)
            : base(message)
        {}
    }
}