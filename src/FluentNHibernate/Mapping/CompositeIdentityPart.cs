using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class CompositeIdentityPart<T> : ICompositeIdMappingProvider
	{
        readonly Action<Member> onMemberMapped;
        readonly AccessStrategyBuilder<CompositeIdentityPart<T>> access;
        readonly AttributeStore attributes = new AttributeStore();
        readonly IList<ICompositeIdKeyMapping> keys = new List<ICompositeIdKeyMapping>();
        bool nextBool = true;

        public CompositeIdentityPart(Action<Member> onMemberMapped)
        {
            this.onMemberMapped = onMemberMapped;
            access = new AccessStrategyBuilder<CompositeIdentityPart<T>>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
        }

        public CompositeIdentityPart(string name, Action<Member> onMemberMapped)
            : this(onMemberMapped)
        {
            attributes.Set("Name", Layer.Defaults, name);
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
            onMemberMapped(member);

            var type = member.PropertyType;

            if (type.IsEnum)
                type = typeof(GenericEnumMapper<>).MakeGenericType(type);

            var key = new KeyPropertyMapping
            {
                ContainingEntityType = typeof(T)
            };
            key.Set(x => x.Name, Layer.Defaults, member.Name);
            key.Set(x => x.Type, Layer.Defaults, new TypeReference(type));

            if (customMapping != null)
            {
                var part = new KeyPropertyPart(key);
                customMapping(part);
            }

            if (!string.IsNullOrEmpty(columnName))
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, columnName);
                key.AddColumn(columnMapping);
            }

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

        protected virtual CompositeIdentityPart<T> KeyReference(Member member, IEnumerable<string> columnNames, Action<KeyManyToOnePart> customMapping)
        {
            onMemberMapped(member);

            var key = new KeyManyToOneMapping
            {
                ContainingEntityType = typeof(T)
            };
            key.Set(x => x.Name, Layer.Defaults, member.Name);
            key.Set(x => x.Class, Layer.Defaults, new TypeReference(member.PropertyType));

            foreach (var column in columnNames)
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, column);
                key.AddColumn(columnMapping);
            }

            var keyPart = new KeyManyToOnePart(key);

            if (customMapping != null)
                customMapping(keyPart);

            keys.Add(key);            

            return this;
        }

		public virtual CompositeIdentityPart<T> CustomType<CType>()
		{
			var key = keys.Where(x => x is KeyPropertyMapping).Cast<KeyPropertyMapping>().LastOrDefault();
			if (key != null)
			{
				key.Set(x => x.Type, Layer.Defaults, new TypeReference(typeof(CType)));
			}
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
            attributes.Set("Mapped", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the unsaved value for the identity
        /// </summary>
        /// <param name="value">Unsaved value</param>
        public CompositeIdentityPart<T> UnsavedValue(string value)
        {
            attributes.Set("UnsavedValue", Layer.UserSupplied, value);
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
            attributes.Set("Class", Layer.Defaults, new TypeReference(typeof(TComponentType)));
            attributes.Set("Name", Layer.Defaults, expression.ToMember().Name);

            return this;
        }

        CompositeIdMapping ICompositeIdMappingProvider.GetCompositeIdMapping()
	    {
            var mapping = new CompositeIdMapping(attributes.Clone())
            {
                ContainingEntityType = typeof(T)
            };

            keys.Each(mapping.AddKey);

	        return mapping;
	    }
	}
}
