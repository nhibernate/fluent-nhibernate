using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FluentNHibernate
{
    public abstract class Member
    {
        public abstract string Name { get; }
        public abstract Type PropertyType { get; }
        public abstract bool CanWrite { get; }
        public abstract MemberInfo MemberInfo { get; }
        public abstract Type DeclaringType { get; }
        public abstract bool HasIndexParameters { get; }
        //   GetIndexParameters().Length == 0
    }

    internal class FieldMember : Member
    {
        private readonly FieldInfo _fieldInfo;

        public FieldMember(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public override string Name
        {
            get { return _fieldInfo.Name; }
        }
        public override Type PropertyType
        {
            get { return _fieldInfo.FieldType; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override MemberInfo MemberInfo
        {
            get { return _fieldInfo; }
        }
        public override Type DeclaringType
        {
            get { return _fieldInfo.DeclaringType; }
        }
        public override bool HasIndexParameters
        {
            get { return false; }
        }
    }

    internal class MethodMember : Member
    {
        private readonly MethodInfo _methodInfo;

        public MethodMember(MethodInfo propertyInfo)
        {
            _methodInfo = propertyInfo;
        }

        public override string Name
        {
            get { return _methodInfo.Name; }
        }
        public override Type PropertyType
        {
            get { return _methodInfo.ReturnType; }
        }
        public override bool CanWrite
        {
            get { return false; }
        }
        public override MemberInfo MemberInfo
        {
            get { return _methodInfo; }
        }
        public override Type DeclaringType
        {
            get { return _methodInfo.DeclaringType; }
        }
        public override bool HasIndexParameters
        {
            get { return false; }
        }
    }

    internal class PropertyMember : Member
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyMember(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public override string Name
        {
            get { return _propertyInfo.Name; }
        }
        public override Type PropertyType
        {
            get { return _propertyInfo.PropertyType; }
        }
        public override bool CanWrite
        {
            get { return _propertyInfo.CanWrite; }
        }
        public override MemberInfo MemberInfo
        {
            get { return _propertyInfo; }
        }
        public override Type DeclaringType
        {
            get { return _propertyInfo.DeclaringType; }
        }
        public override bool HasIndexParameters
        {
            get { return _propertyInfo.GetIndexParameters().Length > 0; }
        }
    }

    public static class MemberExtensions
    {
        public static Member ToMember(this PropertyInfo propertyInfo)
        {
            return new PropertyMember(propertyInfo);
        }
        public static Member ToMember(this MethodInfo methodInfo)
        {
            return new MethodMember(methodInfo);
        }
        public static Member ToMember(this FieldInfo fieldInfo)
        {
            return new FieldMember(fieldInfo);
        }

    }
}
