using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping.Builders
{
    public class CompositeIndexBuilder<T>
    {
        readonly CompositeIndexMapping mapping;

        public CompositeIndexBuilder(CompositeIndexMapping mapping)
        {
            this.mapping = mapping;
            this.mapping.SetDefaultValue(x => x.Type, new TypeReference(typeof(T)));
        }


        /// <summary>
        /// Specifies the type of the element
        /// </summary>
        /// <typeparam name="TElementType">Value type</typeparam>
        public void Type<TElementType>()
        {
            mapping.Type = new TypeReference(typeof(TElementType));
        }

        /// <summary>
        /// Specifies the type of the element
        /// </summary>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the type of the element
        /// </summary>
        /// <param name="type">Type name</param>
        public void Type(string type)
        {
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Defines a property to be used as a key for this composite-id.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <returns>The composite identity part fluent interface</returns>
        public KeyPropertyPart Map(Expression<Func<T, object>> expression)
        {
            var member = expression.ToMember();

            return Map(member, member.Name);
        }

        /// <summary>
        /// Defines a property to be used as a key for this composite-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
        /// <returns>The composite identity part fluent interface</returns>
        public KeyPropertyPart Map(Expression<Func<T, object>> expression, string columnName)
        {
            var member = expression.ToMember();

            return Map(member, columnName);
        }

        KeyPropertyPart Map(Member member, string columnName)
        {
            var type = member.PropertyType;

            if (type.IsEnum)
                type = typeof(GenericEnumMapper<>).MakeGenericType(type);

            var property = new KeyPropertyMapping
            {
                Name = member.Name,
                Type = new TypeReference(type),
                ContainingEntityType = typeof(T)
            };

            if (!string.IsNullOrEmpty(columnName))
                property.AddColumn(new ColumnMapping { Name = columnName });

            mapping.AddProperty(property);

            return new KeyPropertyPart(property);
        }

        /// <summary>
        /// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <returns>The composite identity part fluent interface</returns>
        public KeyManyToOnePart Reference(Expression<Func<T, object>> expression)
        {
            var member = expression.ToMember();

            return Reference(member, new[] { member.Name });
        }

        /// <summary>
        /// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="columnNames">A list of column names used for this key</param>
        /// <returns>The composite identity part fluent interface</returns>
        public KeyManyToOnePart Reference(Expression<Func<T, object>> expression, params string[] columnNames)
        {
            var member = expression.ToMember();

            return Reference(member, columnNames);
        }

        KeyManyToOnePart Reference(Member property, IEnumerable<string> columnNames)
        {
            var reference = new KeyManyToOneMapping
            {
                Name = property.Name,
                Class = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };

            foreach (var column in columnNames)
                reference.AddColumn(new ColumnMapping { Name = column });

            var keyPart = new KeyManyToOnePart(reference);

            mapping.AddReference(reference);

            return new KeyManyToOnePart(reference);
        }
    }
}