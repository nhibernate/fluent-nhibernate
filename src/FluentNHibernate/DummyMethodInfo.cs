using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate
{
    public sealed class DummyMethodInfo : MethodInfo
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
            throw new System.NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new System.NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            throw new System.NotImplementedException();
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new System.NotImplementedException();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new System.NotImplementedException();
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Type ReflectedType
        {
            get { throw new System.NotImplementedException(); }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { throw new System.NotImplementedException(); }
        }

        public override MethodAttributes Attributes
        {
            get { throw new System.NotImplementedException(); }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new System.NotImplementedException();
        }
    }
}