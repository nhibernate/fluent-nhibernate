using System;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.Properties;

namespace FluentNHibernate.Conventions.Instances
{
    public class AccessInstance : IAccessInstance
    {
        private readonly Action<string> setter;

        public AccessInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Property()
        {
            setter("property");
        }

        public void Field()
        {
            setter("field");
        }

        public void BackField()
        {
            setter("backfield");
        }

        public void CamelCaseField()
        {
            CamelCaseField(CamelCasePrefix.None);
        }

        public void CamelCaseField(CamelCasePrefix prefix)
        {
            setter("field.camelcase" + prefix);
        }

        public void LowerCaseField()
        {
            LowerCaseField(LowerCasePrefix.None);
        }

        public void LowerCaseField(LowerCasePrefix prefix)
        {
            setter("field.lowercase" + prefix);
        }

        public void PascalCaseField(PascalCasePrefix prefix)
        {
            setter("field.pascalcase" + prefix);
        }

        public void ReadOnlyProperty()
        {
            setter("no-setter");
        }

        public void ReadOnlyPropertyThroughCamelCaseField()
        {
            ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.None);
        }

        public void ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix)
        {
            setter("nosetter.camelcase" + prefix);
        }

        public void ReadOnlyPropertyThroughLowerCaseField()
        {
            ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix.None);
        }

        public void ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix)
        {
            setter("nosetter.lowercase" + prefix);
        }

        public void ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix)
        {
            setter("nosetter.pascalcase" + prefix);
        }

        public void Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            setter(propertyAccessorAssemblyQualifiedClassName);
        }

        public void Using(Type propertyAccessorClassType)
        {
            Using(propertyAccessorClassType.AssemblyQualifiedName);
        }

        public void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
        {
            Using(typeof(TPropertyAccessorClass));
        }

        public void NoOp()
        {
            setter("noop");
        }

        public void None()
        {
            setter("none");
        }
    }
}