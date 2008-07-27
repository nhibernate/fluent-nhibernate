using System;
using NHibernate.Properties;

namespace ShadeTree.DomainModel.Mapping
{
    public class AccessStrategyBuilder
    {
        private readonly PropertyMap parent;

        public AccessStrategyBuilder(PropertyMap parent)
        {
            this.parent = parent;
        }

        public PropertyMap AsProperty()
        {
            SetAccess("property");

            return parent;
        }

        public PropertyMap AsField()
        {
            SetAccess("field");

            return parent;
        }

        public PropertyMap AsCamelCaseField()
        {
            AsCamelCaseField(Prefix.None);

            return parent;
        }

        public PropertyMap AsCamelCaseField(Prefix prefix)
        {
            SetAccess("field.camelcase" + prefix.Value);

            return parent;
        }

        public PropertyMap AsLowerCaseField()
        {
            AsLowerCaseField(Prefix.None);

            return parent;
        }

        public PropertyMap AsLowerCaseField(Prefix prefix)
        {
            SetAccess("field.lowercase" + prefix.Value);

            return parent;
        }

        public PropertyMap AsPascalCaseField(Prefix prefix)
        {
            SetAccess("field.pascalcase" + prefix.Value);
            
            return parent;
        }

        public PropertyMap AsReadOnlyPropertyThroughCamelCaseField()
        {
            AsReadOnlyPropertyThroughCamelCaseField(Prefix.None);

            return parent;
        }

        public PropertyMap AsReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            SetAccess("nosetter.camelcase" + prefix.Value);

            return parent;
        }

        public PropertyMap AsReadOnlyPropertyThroughLowerCaseField()
        {
            AsReadOnlyPropertyThroughLowerCaseField(Prefix.None);

            return parent;
        }

        public PropertyMap AsReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            SetAccess("nosetter.lowercase" + prefix.Value);

            return parent;
        }

        public PropertyMap AsReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            SetAccess("nosetter.pascalcase" + prefix.Value);

            return parent;
        }

        public PropertyMap Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            SetAccess(propertyAccessorAssemblyQualifiedClassName);

            return parent;
        }

        public PropertyMap Using(Type propertyAccessorClassType)
        {
            Using(propertyAccessorClassType.AssemblyQualifiedName);

            return parent;
        }

        public PropertyMap Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
        {
            Using(typeof(TPropertyAccessorClass));

            return parent;
        }

        private void SetAccess(string value)
        {
            parent.SetAttributeOnPropertyElement("access", value);
        }
    }
}