using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Automapping
{
    [Serializable]
    public class AutoMappingException : Exception
    {
        public AutoMappingException(string message)
            : base(message)
        {}

        protected AutoMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}