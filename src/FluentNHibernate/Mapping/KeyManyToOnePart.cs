using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping;

public class KeyManyToOnePart
{
    readonly KeyManyToOneMapping mapping;
    bool nextBool = true;

    public KeyManyToOnePart(KeyManyToOneMapping mapping)
    {
        this.mapping = mapping;
        Access = new AccessStrategyBuilder<KeyManyToOnePart>(this, value => mapping.Set(x => x.Access, Layer.UserSupplied, value));
        NotFound = new NotFoundExpression<KeyManyToOnePart>(this, value => mapping.Set(x => x.NotFound, Layer.UserSupplied, value));
    }

    /// <summary>
    /// Inverts the next boolean
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyManyToOnePart Not
    {
        get
        {
            nextBool = !nextBool;
            return this;
        }
    }

    public KeyManyToOnePart ForeignKey(string foreignKey)
    {
        mapping.Set(x => x.ForeignKey, Layer.UserSupplied, foreignKey);
        return this;
    }

    /// <summary>
    /// Defines how NHibernate will access the object for persisting/hydrating (Defaults to Property)
    /// </summary>
    public AccessStrategyBuilder<KeyManyToOnePart> Access { get; }

    public NotFoundExpression<KeyManyToOnePart> NotFound { get; }

    public KeyManyToOnePart Lazy()
    {
        mapping.Set(x => x.Lazy, Layer.UserSupplied, nextBool);
        nextBool = true;
        return this;
    }

    public KeyManyToOnePart Name(string name)
    {
        mapping.Set(x => x.Name, Layer.UserSupplied, name);
        return this;
    }

    /// <summary>
    /// Specifies an entity-name.
    /// </summary>
    /// <remarks>See https://nhibernate.info/blog/2008/10/21/entity-name-in-action-a-strongly-typed-entity.html </remarks>
    public KeyManyToOnePart EntityName(string entityName)
    {
        mapping.Set(x => x.EntityName, Layer.UserSupplied, entityName);
        return this;
    }

    /// <summary>
    /// Specifies the child class of this key/relationship
    /// </summary>
    /// <typeparam name="T">Child</typeparam>
    public KeyManyToOnePart Class<T>()
    {
        return Class(typeof(T));
    }

    /// <summary>
    /// Specifies the child class of this key/relationship
    /// </summary>
    /// <param name="type">Child</param>
    public KeyManyToOnePart Class(Type type)
    {
        mapping.Set(x => x.Class, Layer.UserSupplied, new TypeReference(type));
        return this;
    }
}
