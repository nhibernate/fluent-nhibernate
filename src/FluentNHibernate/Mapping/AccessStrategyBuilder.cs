using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Access strategy mapping builder.
    /// </summary>
    /// <typeparam name="T">Mapping part to be applied to</typeparam>
    public class AccessStrategyBuilder<T> : IAccessStrategyBuilder
        where T : IHasAttributes
    {
        private const string InvalidPrefixCamelCaseFieldM = "m is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixCamelCaseFieldMUnderscore = "m_ is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixLowerCaseFieldM = "m is not a valid prefix for a LowerCase Field.";
        private const string InvalidPrefixLowerCaseFieldMUnderscore = "m_ is not a valid prefix for a LowerCase Field.";
        private const string InvalidPrefixPascalCaseFieldNone = "None is not a valid prefix for a PascalCase Field.";

        private readonly T parent;

        /// <summary>
        /// Access strategy mapping builder.
        /// </summary>
        /// <param name="parent">Instance of the parent mapping part.</param>
        public AccessStrategyBuilder(T parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Sets the access-strategy to property.
        /// </summary>
        public T AsProperty()
        {
            SetAccessAttribute("property");

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        public T AsField()
        {
            SetAccessAttribute("field");

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the backing-field of an auto-property.
        /// </summary>
        /// <returns></returns>
        public T AsBackingField()
        {
            SetAccessAttribute("backfield");

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        public T AsCamelCaseField()
        {
            AsCamelCaseField(Prefix.None);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsCamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            SetAccessAttribute("field.camelcase" + prefix.Value);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        public T AsLowerCaseField()
        {
            AsLowerCaseField(Prefix.None);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsLowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            SetAccessAttribute("field.lowercase" + prefix.Value);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsPascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            SetAccessAttribute("field.pascalcase" + prefix.Value);
            
            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        public T AsReadOnlyPropertyThroughCamelCaseField()
        {
            AsReadOnlyPropertyThroughCamelCaseField(Prefix.None);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            SetAccessAttribute("nosetter.camelcase" + prefix.Value);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        public T AsReadOnlyPropertyThroughLowerCaseField()
        {
            AsReadOnlyPropertyThroughLowerCaseField(Prefix.None);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            SetAccessAttribute("nosetter.lowercase" + prefix.Value);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public T AsReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            SetAccessAttribute("nosetter.pascalcase" + prefix.Value);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorAssemblyQualifiedClassName">Assembly qualified name of the type to use as the access-strategy</param>
        public T Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            SetAccessAttribute(propertyAccessorAssemblyQualifiedClassName);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorClassType">Type to use as the access-strategy</param>
        public T Using(Type propertyAccessorClassType)
        {
            Using(propertyAccessorClassType.AssemblyQualifiedName);

            return parent;
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <typeparam name="TPropertyAccessorClass">Type to use as the access-strategy</typeparam>
        public T Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
        {
            Using(typeof(TPropertyAccessorClass));

            return parent;
        }

        /// <summary>
        /// Sets the access attribute on the parent element.
        /// </summary>
        /// <param name="value">Value for the access attribute</param>
        protected virtual void SetAccessAttribute(string value)
        {
            parent.SetAttribute("access", value);
        }

        void IAccessStrategyBuilder.AsProperty()
        {
            AsProperty();
        }

        void IAccessStrategyBuilder.AsField()
        {
            AsField();
        }

        void IAccessStrategyBuilder.AsCamelCaseField()
        {
            AsCamelCaseField();
        }

        void IAccessStrategyBuilder.AsCamelCaseField(Prefix prefix)
        {
            AsCamelCaseField(prefix);
        }

        void IAccessStrategyBuilder.AsLowerCaseField()
        {
            AsLowerCaseField();
        }

        void IAccessStrategyBuilder.AsLowerCaseField(Prefix prefix)
        {
            AsLowerCaseField(prefix);
        }

        void IAccessStrategyBuilder.AsPascalCaseField(Prefix prefix)
        {
            AsPascalCaseField(prefix);
        }

        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughCamelCaseField()
        {
            AsReadOnlyPropertyThroughCamelCaseField();
        }

        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            AsReadOnlyPropertyThroughCamelCaseField(prefix);
        }

        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughLowerCaseField()
        {
            AsReadOnlyPropertyThroughLowerCaseField();
        }

        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            AsReadOnlyPropertyThroughLowerCaseField(prefix);
        }

        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            AsReadOnlyPropertyThroughPascalCaseField(prefix);
        }

        void IAccessStrategyBuilder.Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            Using(propertyAccessorAssemblyQualifiedClassName);
        }

        void IAccessStrategyBuilder.Using(Type propertyAccessorClassType)
        {
            Using(propertyAccessorClassType);
        }

        void IAccessStrategyBuilder.Using<TPropertyAccessorClass>()
        {
            Using<TPropertyAccessorClass>();
        }
    }
}