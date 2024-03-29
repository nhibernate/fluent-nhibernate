using System;

namespace FluentNHibernate.Visitors;

[Serializable]
public class ValidationException(string message, string resolution, Type relatedEntity)
    : Exception(message + " " + resolution + ".")
{
    public Type RelatedEntity { get; } = relatedEntity;
    public string Resolution { get; } = resolution;
}
