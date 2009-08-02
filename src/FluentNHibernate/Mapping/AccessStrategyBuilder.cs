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
        public new T Property()
        {
            base.Property();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        public new T Field()
        {
            base.Field();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the backing-field of an auto-property.
        /// </summary>
        public new T BackingField()
        {
            base.BackingField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        public new T CamelCaseField()
        {
            base.CamelCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public new T CamelCaseField(Prefix prefix)
        {
            base.CamelCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        public new T LowerCaseField()
        {
            base.LowerCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public new T LowerCaseField(Prefix prefix)
        {
            base.LowerCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public new T PascalCaseField(Prefix prefix)
        {
            base.PascalCaseField(prefix);
            
            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        public new T ReadOnlyPropertyThroughCamelCaseField()
        {
            base.ReadOnlyPropertyThroughCamelCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public new T ReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            base.ReadOnlyPropertyThroughCamelCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        public new T ReadOnlyPropertyThroughLowerCaseField()
        {
            base.ReadOnlyPropertyThroughLowerCaseField();

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public new T ReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            base.ReadOnlyPropertyThroughLowerCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public new T ReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            base.ReadOnlyPropertyThroughPascalCaseField(prefix);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorAssemblyQualifiedClassName">Assembly qualified name of the type to use as the access-strategy</param>
        public new T Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            base.Using(propertyAccessorAssemblyQualifiedClassName);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorClassType">Type to use as the access-strategy</param>
        public new T Using(Type propertyAccessorClassType)
        {
            base.Using(propertyAccessorClassType);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <typeparam name="TPropertyAccessorClass">Type to use as the access-strategy</typeparam>
        public new T Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
        {
            base.Using<TPropertyAccessorClass>();

            return parent;
        }

        public new T NoOp()
        {
            setValue("noop");

            return parent;
        }

        public new T None()
        {
            setValue("none");

            return parent;
        }
    }
}