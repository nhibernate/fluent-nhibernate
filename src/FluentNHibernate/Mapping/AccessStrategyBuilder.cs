using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Access strategy mapping builder.
    /// </summary>
    /// <typeparam name="T">Mapping part to be applied to</typeparam>
    public class AccessStrategyBuilder<T> : AccessStrategyBuilder
    {
        private readonly T parent;

        /// <summary>
        /// Access strategy mapping builder.
        /// </summary>
        /// <param name="parent">Instance of the parent mapping part.</param>
        /// <param name="setter">Setter for altering the model</param>
        public AccessStrategyBuilder(T parent, Action<string> setter)
            : base(setter)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Sets the access-strategy to property.
        /// </summary>
        public T Property()
        {
            ((IAccessStrategyBuilder)this).Property();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        public T Field()
        {
            ((IAccessStrategyBuilder)this).Field();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        public T CamelCaseField()
        {
            ((IAccessStrategyBuilder)this).CamelCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T CamelCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).CamelCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        public T LowerCaseField()
        {
            ((IAccessStrategyBuilder)this).LowerCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T LowerCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).LowerCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T PascalCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).PascalCaseField(prefix);
            
            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        public T ReadOnlyPropertyThroughCamelCaseField()
        {
            ((IAccessStrategyBuilder)this).ReadOnlyPropertyThroughCamelCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T ReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).ReadOnlyPropertyThroughCamelCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        public T ReadOnlyPropertyThroughLowerCaseField()
        {
            ((IAccessStrategyBuilder)this).ReadOnlyPropertyThroughLowerCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T ReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).ReadOnlyPropertyThroughLowerCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T ReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            ((IAccessStrategyBuilder)this).ReadOnlyPropertyThroughPascalCaseField(prefix);

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