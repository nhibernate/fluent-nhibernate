using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public CompositeIdentityPart(string name) : this()
        {
            attributes.Set(x => x.Name, name);
        }

        /// <summary>
		/// Defines a property to be used as a key for this composite-id.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyProperty(Expression<Func<T, object>> expression)
		{
	        var member = ReflectionHelper.GetMember(expression);

            return KeyProperty(member, member.Name, null);
		}

		/// <summary>
		/// Defines a property to be used as a key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyProperty(Expression<Func<T, object>> expression, string columnName)
		{
            var member = ReflectionHelper.GetMember(expression);

		    return KeyProperty(member, columnName, null);
		}

        /// <summary>
        /// Defines a property to be used as a key for this composite-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>        
        /// <param name="keyPropertyAction">Additional settings for the key property</param>
        /// <returns>The composite identity part fluent interface</returns>
        public CompositeIdentityPart<T> KeyProperty(Expression<Func<T, object>> expression, Action<KeyPropertyPart> keyPropertyAction)
        {
            var member = ReflectionHelper.GetMember(expression);
            return KeyProperty(member, string.Empty, keyPropertyAction);
        }

        protected virtual CompositeIdentityPart<T> KeyProperty(Member property, string columnName, Action<KeyPropertyPart> customMapping)
        {
            var key = new KeyPropertyMapping
            {
                Name = property.Name,
                Type = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };

            if (customMapping != null)
            {
                var part = new KeyPropertyPart(key);
                customMapping(part);
            }

            if(!string.IsNullOrEmpty(columnName))
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
		    var member = ReflectionHelper.GetMember(expression);

		    return KeyReference(member, member.Name, null);
		}

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression, string columnName)
		{
            var member = ReflectionHelper.GetMember(expression);

            return KeyReference(member, columnName, null);
		}


        /// <summary>
        /// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
        /// <param name="customMapping">A lambda expression specifying additional settings for the key reference</param>
        /// <returns>The composite identity part fluent interface</returns>
        public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression, string columnName, Action<KeyManyToOnePart> customMapping)
        {
            var member = ReflectionHelper.GetMember(expression);

            return KeyReference(member, columnName, customMapping);
        }

        protected virtual CompositeIdentityPart<T> KeyReference(Member property, string columnName, Action<KeyManyToOnePart> customMapping)
        {
            var key = new KeyManyToOneMapping
            {
                Name = property.Name,
                Class = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };
            key.AddColumn(new ColumnMapping { Name = columnName });

            var keyPart = new KeyManyToOnePart(key);

            if (customMapping != null)
                customMapping(keyPart);

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
