using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class TuplizerPart
    {
        readonly TuplizerMapping mapping;

        public TuplizerPart(TuplizerMapping mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// Sets the tuplizer type.
        /// </summary>
        /// <param name="type">Type</param>
        public TuplizerPart Type(Type type)
        {
            mapping.Type = new TypeReference(type);
            return this;
        }

        /// <summary>
        /// Sets the tuplizer type.
        /// </summary>
        /// <param name="type">Type</param>
        public TuplizerPart Type(string type)
        {
            mapping.Type = new TypeReference(type);
            return this;
        }

        /// <summary>
        /// Sets the tuplizer type.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public TuplizerPart Type<T>()
        {
            return Type(typeof(T));
        }

        /// <summary>
        /// Sets the tuplizer mode
        /// </summary>
        /// <param name="mode">Mode</param>
        public TuplizerPart Mode(TuplizerMode mode)
        {
            mapping.Mode = mode;
            return this;
        }

        /// <summary>
        /// Specifies an entity-name.
        /// </summary>
        /// <remarks>See http://nhforge.org/blogs/nhibernate/archive/2008/10/21/entity-name-in-action-a-strongly-typed-entity.aspx</remarks>
        public TuplizerPart EntityName(string entityName)
        {
            mapping.EntityName = entityName;
            return this;
        }
    }
}