using System;

namespace FluentNHibernate.Automapping
{
    [Serializable]
    public class AutoMappingException : Exception
    {
        public AutoMappingException(string message)
            : base(message)
        {}
    }
}