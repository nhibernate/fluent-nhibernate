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
        private readonly object _conventionTarget;

        public ConventionException(string message, object conventionTarget) : base(message)
        {
            _conventionTarget = conventionTarget;
        }

        protected ConventionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public object ConventionTarget
        {
            get { return _conventionTarget; }
        }
    }
}
