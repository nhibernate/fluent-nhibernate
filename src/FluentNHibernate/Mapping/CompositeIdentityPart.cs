using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Mapping
{
    public class CompositeIdentityPart<T> : ICompositeIdMappingProvider
	{
        private readonly AccessStrategyBuilder<CompositeIdentityPart<T>> access;
        private readonly AttributeStore<CompositeIdMapping> attributes = new AttributeStore<CompositeIdMapping>();
        private readonly IList<ICompositeIdKeyMapping> keys = new List<ICompositeIdKeyMapping>();
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
            var member = expression.ToMember();

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
            var member = expression.ToMember();

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
            var member = expression.ToMember();
            return KeyProperty(member, string.Empty, keyPropertyAction);
        }

        protected virtual CompositeIdentityPart<T> KeyProperty(Member member, string columnName, Action<KeyPropertyPart> customMapping)
        {
            var type = member.PropertyType;

            if (type.IsEnum)
                type = typeof(GenericEnumMapper<>).MakeGenericType(type);

            var key = new KeyPropertyMapping
            {
                Name = member.Name,
                Type = new TypeReference(type),
                ContainingEntityType = typeof(T)
            };

            if (customMapping != null)
            {
                var part = new KeyPropertyPart(key);
                customMapping(part);
            }

            if(!string.IsNullOrEmpty(columnName))
                key.AddColumn(new ColumnMapping { Name = columnName });

            keys.Add(key);

            return this;
        }

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression)
		{
            var member = expression.ToMember();

		    return KeyReference(member, new[] { member.Name }, null);
		}

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="columnNames">A list of column names used for this key</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression, params string[] columnNames)
		{
            var member = expression.ToMember();

            return KeyReference(member, columnNames, null);
		}


        /// <summary>
        /// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="customMapping">A lambda expression specifying additional settings for the key reference</param>
        /// <param name="columnNames">A list of column names used for this key</param>
        /// <returns>The composite identity part fluent interface</returns>
        public CompositeIdentityPart<T> KeyReference(Expression<Func<T, object>> expression, Action<KeyManyToOnePart> customMapping, params string[] columnNames)
        {
            var member = expression.ToMember();

            return KeyReference(member, columnNames, customMapping);
        }

        protected virtual CompositeIdentityPart<T> KeyReference(Member property, IEnumerable<string> columnNames, Action<KeyManyToOnePart> customMapping)
        {
            var key = new KeyManyToOneMapping
            {
                Name = property.Name,
                Class = new TypeReference(property.PropertyType),
                ContainingEntityType = typeof(T)
            };

            foreach (var column in columnNames)
                key.AddColumn(new ColumnMapping { Name = column });

            var keyPart = new KeyManyToOnePart(key);

            if (customMapping != null)
                customMapping(keyPart);

            keys.Add(key);            

            return this;
        }

		/// <summary>
		/// Set the access and naming strategy for this identity.
		/// </summary>
		public AccessStrategyBuilder<CompositeIdentityPart<T>> Access
		{
			get { return access; }
		}

        /// <summary>
        /// Invert the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public CompositeIdentityPart<T> Not
        {
            get
            {
                nextBool = false;
                return this;
            }
        }

        /// <summary>
        /// Specifies that this composite id is "mapped"; aka, a composite id where
        /// the properties exist in the identity class as well as in the entity itself
        /// </summary>
        public CompositeIdentityPart<T> Mapped()
        {
            attributes.Set(x => x.Mapped, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the unsaved value for the identity
        /// </summary>
        /// <param name="value">Unsaved value</param>
        public CompositeIdentityPart<T> UnsavedValue(string value)
        {
            attributes.Set(x => x.UnsavedValue, value);
            return this;
        }

        /// <summary>
        /// You may use a component as an identifier of an entity class. Your component class must
        /// satisfy certain requirements:
        ///
        ///   * It must be Serializable.
        ///   * It must re-implement Equals() and GetHashCode(), consistently with the database's
        ///     notion of composite key equality. 
        /// 
        /// You can't use an IIdentifierGenerator to generate composite keys. Instead the application
        /// must assign its own identifiers. Since a composite identifier must be assigned to the object
        /// before saving it, we can't use unsaved-value of the identifier to distinguish between newly
        /// instantiated instances and instances saved in a previous session. You may instead implement
        /// IInterceptor.IsUnsaved() if you wish to use SaveOrUpdate() or cascading save / update. As an
        /// alternative, you may also set the unsaved-value attribute on a version or timestamp to specify
        /// a value that indicates a new transient instance. In this case, the version of the entity is
        /// used instead of the (assigned) identifier and you don't have to implement
        /// IInterceptor.IsUnsaved() yourself. 
        /// </summary>
        /// <param name="expression">The property of component type that holds the composite identifier.</param>        
        /// <remarks>
        /// Your persistent class must override Equals() and GetHashCode() to implement composite identifier
        /// equality. It must also be Serializable.
        /// </remarks>
        public CompositeIdentityPart<T> ComponentCompositeIdentifier<TComponentType>(Expression<Func<T, TComponentType>> expression)
        {
            attributes.Set(x => x.Class, new TypeReference(typeof(TComponentType)));
            attributes.Set(x => x.Name, ReflectionHelper.GetMember(expression).Name);

            return this;
        }

        CompositeIdMapping ICompositeIdMappingProvider.GetCompositeIdMapping()
	    {
            var mapping = new CompositeIdMapping(attributes.CloneInner());

	        mapping.ContainingEntityType = typeof(T);

            keys.Each(mapping.AddKey);

	        return mapping;
	    }
	}
}
