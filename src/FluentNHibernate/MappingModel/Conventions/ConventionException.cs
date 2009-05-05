using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.MappingModel.Conventions
{
    [Serializable]
    public class ConventionException : Exception
    {
        private readonly object conventionTarget;

        public ConventionException(string message, object conventionTarget) : base(message)
        {
            this.conventionTarget = conventionTarget;
        }

        protected ConventionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public object ConventionTarget
        {
            get { return conventionTarget; }
        }
    }
}
