using System.Diagnostics;
using FluentNHibernate.MappingModel;
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
        public AccessStrategyBuilder<KeyManyToOnePart> Access { get; private set; }

        public NotFoundExpression<KeyManyToOnePart> NotFound { get; private set; }

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
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public KeyManyToOnePart EntityName(string entityName)
        {
            mapping.Set(x => x.EntityName, Layer.UserSupplied, entityName);
            return this;
        }
    }
}