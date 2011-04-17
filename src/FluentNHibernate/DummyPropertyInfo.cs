using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate
{
    [Serializable]
    public sealed class DummyPropertyInfo : PropertyInfo
    {
        private readonly string name;
        private readonly Type type;

        public DummyPropertyInfo(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (type == null) throw new ArgumentNullException("type");

            this.name = name;
            this.type = type;
        }

        public override Module Module
        {
            get { return null; }
        }

        public override int MetadataToken
        {
            get { return name.GetHashCode(); }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return false;
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            return obj;
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {}

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return new MethodInfo[0];
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return null;
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return null;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            return new ParameterInfo[0];
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { return type; }
        }

        public override Type ReflectedType
        {
            get { return null; }
        }

        public override Type PropertyType
        {
            get { return type; }
        }

        public override PropertyAttributes Attributes
        {
            get { return PropertyAttributes.None; }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return new object[0];
        }
    }
}
