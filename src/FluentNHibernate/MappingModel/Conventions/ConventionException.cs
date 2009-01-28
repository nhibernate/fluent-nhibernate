using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FluentNHibernate.MappingModel.Conventions
{
    [Serializable]
    public class ConventionException : Exception
    {
        public ConventionException()
        {
        }

        public ConventionException(string message) : base(message)
        {
        }

        public ConventionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConventionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
