using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Access strategy mapping builder.
    /// </summary>
    /// <typeparam name="T">Mapping part to be applied to</typeparam>
    public class AccessStrategyBuilder<T> : AccessStrategyBuilder
        where T : IHasAttributes
    {
        private readonly T parent;

        /// <summary>
        /// Access strategy mapping builder.
        /// </summary>
        /// <param name="parent">Instance of the parent mapping part.</param>
        public AccessStrategyBuilder(T parent)
            : this(parent, value => parent.SetAttribute("access", value))
        {}

        protected AccessStrategyBuilder(T parent, Action<string> setter)
            : base(setter)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Sets the access-strategy to property.
        /// </summary>
        public T AsProperty()
        {
            ((IAccessStrategyBuilder)this).AsProperty();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        public T AsField()
        {
            ((IAccessStrategyBuilder)this).AsField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        public T AsCamelCaseField()
        {
            ((IAccessStrategyBuilder)this).AsCamelCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsCamelCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).AsCamelCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        public T AsLowerCaseField()
        {
            ((IAccessStrategyBuilder)this).AsLowerCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsLowerCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).AsLowerCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsPascalCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).AsPascalCaseField(prefix);
            
            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        public T AsReadOnlyPropertyThroughCamelCaseField()
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughCamelCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughCamelCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        public T AsReadOnlyPropertyThroughLowerCaseField()
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughLowerCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughLowerCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughPascalCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorAssemblyQualifiedClassName">Assembly qualified name of the type to use as the access-strategy</param>
        public T Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            ((IAccessStrategyBuilder)this).Using(propertyAccessorAssemblyQualifiedClassName);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorClassType">Type to use as the access-strategy</param>
        public T Using(Type propertyAccessorClassType)
        {
            ((IAccessStrategyBuilder)this).Using(propertyAccessorClassType);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <typeparam name="TPropertyAccessorClass">Type to use as the access-strategy</typeparam>
        public T Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
        {
            ((IAccessStrategyBuilder)this).Using<TPropertyAccessorClass>();

            return parent;
        }
    }
}