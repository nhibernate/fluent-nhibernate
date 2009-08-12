using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class CompositeIdentityPart<T> : ICompositeIdMappingProvider
	{
        private readonly AccessStrategyBuilder<CompositeIdentityPart<T>> access;
        private readonly AttributeStore<CompositeIdMapping> attributes = new AttributeStore<CompositeIdMapping>();
        private readonly IList<KeyPropertyMapping> keyProperties = new List<KeyPropertyMapping>();
        private readonly IList<KeyManyToOneMapping> keyManyToOnes = new List<KeyManyToOneMapping>();
        private bool nextBool = true;

        public CompositeIdentityPart()
        {
            access = new AccessStrategyBuilder<CompositeIdentityPart<T>>(this, value => attributes.Set(x => x.Access, value));
        }

        /// <summary>
		/// Defines a property to be used as a key for this composite-id.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyProperty(Expression<Func<T, object>> expression)
		{
	        var property = ReflectionHelper.GetProperty(expression);

			return KeyProperty(property, property.Name);
		}

		/// <summary>
		/// Defines a property to be used as a key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyProperty(Expression<Func<T, object>> expression, string columnName)
		{
            var property = ReflectionHelper.GetProperty(expression);

		    return KeyProperty(property, columnName);
		}

        private CompositeIdentityPart<T> KeyProperty(PropertyInfo property, string columnName)
        {
            var key = new KeyPropertyMapping
            {
                Name = property.Name,
                Type = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };
            key.AddColumn(new ColumnMapping { Name = columnName });

            keyProperties.Add(key);

            return this;
        }

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression)
		{
		    var property = ReflectionHelper.GetProperty(expression);

		    return KeyReference(property, property.Name);
		}

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression, string columnName)
		{
            var property = ReflectionHelper.GetProperty(expression);

            return KeyReference(property, columnName);
		}

        private CompositeIdentityPart<T> KeyReference(PropertyInfo property, string columnName)
        {
            var key = new KeyManyToOneMapping
            {
                Name = property.Name,
                Class = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };
            key.AddColumn(new ColumnMapping { Name = columnName });

            keyManyToOnes.Add(key);

            return this;
        }

		/// <summary>
		/// Set the access and naming strategy for this identity.
		/// </summary>
		public AccessStrategyBuilder<CompositeIdentityPart<T>> Access
		{
			get { return access; }
		}

        public CompositeIdentityPart<T> Not
        {
            get
            {
                nextBool = false;
                return this;
            }
        }

        public CompositeIdentityPart<T> Mapped()
        {
            attributes.Set(x => x.Mapped, nextBool);
            nextBool = true;
            return this;
        }

        public CompositeIdentityPart<T> UnsavedValue(string value)
        {
            attributes.Set(x => x.UnsavedValue, value);
            return this;
        }

	    CompositeIdMapping ICompositeIdMappingProvider.GetCompositeIdMapping()
	    {
            var mapping = new CompositeIdMapping(attributes.CloneInner());

	        mapping.ContainingEntityType = typeof(T);

            keyProperties.Each(mapping.AddKeyProperty);
            keyManyToOnes.Each(mapping.AddKeyManyToOne);

	        return mapping;
	    }
	}
}
