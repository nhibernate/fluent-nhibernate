using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Builders
{
    public class OneToManyBuilder
    {
        readonly OneToManyMapping mapping;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();

        public OneToManyBuilder(OneToManyMapping mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <typeparam name="TChild">Child type</typeparam>
        public void Type<TChild>()
        {
            mapping.Class = new TypeReference(typeof(TChild));
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            mapping.Class = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <param name="type">Type name</param>
        public void Type(string type)
        {
            mapping.Class = new TypeReference(type);
        }

        /// <summary>
        /// Specify the not-found behaviour for the relationship
        /// </summary>
        public NotFoundBuilder NotFound
        {
            get { return new NotFoundBuilder(value => mapping.NotFound = value);}
        }

        /// <summary>
        /// Specifies the entity-name for the relationship
        /// </summary>
        /// <param name="entityName">Type name</param>
        public void EntityName(string entityName)
        {
            mapping.EntityName = entityName;
        }
    }
}