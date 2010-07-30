using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public class KeyManyToOnePart
    {
        private readonly KeyManyToOneMapping mapping;
        private bool nextBool = true;

        public KeyManyToOnePart(KeyManyToOneMapping mapping)
        {
            this.mapping = mapping;
            Access = new AccessStrategyBuilder<KeyManyToOnePart>(this, value => mapping.Access = value);
            NotFound = new NotFoundExpression<KeyManyToOnePart>(this, value => mapping.NotFound = value);
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
            mapping.ForeignKey = foreignKey;
            return this;
        }

        /// <summary>
        /// Defines how NHibernate will access the object for persisting/hydrating (Defaults to Property)
        /// </summary>
        public AccessStrategyBuilder<KeyManyToOnePart> Access { get; private set; }

        public NotFoundExpression<KeyManyToOnePart> NotFound { get; private set; }

        public KeyManyToOnePart Lazy()
        {
            mapping.Lazy = nextBool;
            nextBool = true;
            return this;
        }

        public KeyManyToOnePart Name(string name)
        {
            mapping.Name = name;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public KeyManyToOnePart EntityName(string entityName)
        {
            mapping.EntityName = entityName;
            return this;
        }
    }
}