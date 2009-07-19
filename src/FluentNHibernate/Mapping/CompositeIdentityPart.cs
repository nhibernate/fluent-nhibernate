using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class CompositeIdentityPart<T> : IAccessStrategy<CompositeIdentityPart<T>>, ICompositeIdMappingProvider
	{
        private readonly AccessStrategyBuilder<CompositeIdentityPart<T>> access;
        private readonly CompositeIdMapping mapping = new CompositeIdMapping();

        public CompositeIdentityPart()
        {
            access = new AccessStrategyBuilder<CompositeIdentityPart<T>>(this, value => mapping.Access = value);
        }

        /// <summary>
		/// Defines a property to be used as a key for this composite-id.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyProperty(Expression<Func<T, object>> expression)
		{
	        var property = ReflectionHelper.GetProperty(expression);

			return WithKeyProperty(property, property.Name);
		}

		/// <summary>
		/// Defines a property to be used as a key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyProperty(Expression<Func<T, object>> expression, string columnName)
		{
            var property = ReflectionHelper.GetProperty(expression);

		    return WithKeyProperty(property, columnName);
		}

        private CompositeIdentityPart<T> WithKeyProperty(PropertyInfo property, string columnName)
        {
            var key = new KeyPropertyMapping { Name = property.Name, Type = new TypeReference(property.PropertyType) };
            key.AddColumn(new ColumnMapping { Name = columnName });

            mapping.AddKeyProperty(key);

            return this;
        }

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyReference(Expression<Func<T, object>> expression)
		{
		    var property = ReflectionHelper.GetProperty(expression);

		    return WithKeyReference(property, property.Name);
		}

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyReference(Expression<Func<T, object>> expression, string columnName)
		{
            var property = ReflectionHelper.GetProperty(expression);

            return WithKeyReference(property, columnName);
		}

        private CompositeIdentityPart<T> WithKeyReference(PropertyInfo property, string columnName)
        {
            var key = new KeyManyToOneMapping
            {
                Name = property.Name,
                Class = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };
            key.AddColumn(new ColumnMapping { Name = columnName });

            mapping.AddKeyManyToOne(key);

            return this;
        }

		/// <summary>
		/// Set the access and naming strategy for this identity.
		/// </summary>
		public AccessStrategyBuilder<CompositeIdentityPart<T>> Access
		{
			get { return access; }
		}

	    CompositeIdMapping ICompositeIdMappingProvider.GetCompositeIdMapping()
	    {
	        mapping.ContainingEntityType = typeof(T);
	        return mapping;
	    }
	}
}
