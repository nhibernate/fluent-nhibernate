using System;

namespace FluentNHibernate.Automapping
{
    public class AutoMappingException : Exception
    {
        public AutoMappingException(string message)
            : base(message)
        {}
    }
}