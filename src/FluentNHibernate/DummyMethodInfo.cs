using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate
{
    internal sealed class DummyMethodInfo : MethodInfo
    {
        private readonly string name;
        private readonly Type type;

        public DummyMethodInfo(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        public override Type ReturnType
        {
            get { return type; }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override ParameterInfo[] GetParameters()
        {
            return new ParameterInfo[0];
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return MethodImplAttributes.Managed;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return obj;
        }

        public override MethodInfo GetBaseDefinition()
        {
            return null;
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { return null; }
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { return null; }
        }

        public override Type ReflectedType
        {
            get { return null; }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { return new RuntimeMethodHandle(); }
        }

        public override MethodAttributes Attributes
        {
            get { return MethodAttributes.Public; }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[0];
        }
    }
}