using System;

namespace FluentNHibernate.Visitors
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(string message, string resolution, Type relatedEntity)
            : base(message + " " + resolution + ".")
        {
            Resolution = resolution;
            RelatedEntity = relatedEntity;
        }

        public Type RelatedEntity { get; private set; }
        public string Resolution { get; private set; }
    }
}
