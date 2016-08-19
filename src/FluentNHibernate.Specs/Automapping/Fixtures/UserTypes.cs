using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public class TypeWithNullableUT
    {
        public int ID { get; set; }
        public UserValueType? Simple { get; set; }
    }

    public class TypeWithValueUT
    {
        public int Id { get; set; }
        public UserValueType Definite { get; set; }
    }

    public class NotNullableUT
    {
        public int ID { get; set; }
        public CustomUserType Complex { get; set; }
    }

    public class ValueTypeConvention : UserTypeConvention<UserValueType>
    {
        public override void Apply(IPropertyInstance instance)
        {
            base.Apply(instance);
            instance.Column("arbitraryName");
        }
    }

    public class CustomTypeConvention : UserTypeConvention<CustomUserType>
    {
        public override void Apply(IPropertyInstance instance)
        {
            base.Apply(instance);
            instance.Column("someOtherName");
        }
    }

    public class CustomUserType : IUserType
    {
        public new bool Equals(object x, object y)
        {
            return x == y;
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            return null;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {

        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes
        {
            get { return new[] { new SqlType(DbType.String) }; }
        }

        public Type ReturnedType
        {
            get { return typeof(CustomUserType); }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }

    public struct UserValueType : IUserType
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(object x)
        {
            throw new NotImplementedException();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            throw new NotImplementedException();
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            throw new NotImplementedException();
        }

        public object DeepCopy(object value)
        {
            throw new NotImplementedException();
        }

        public object Replace(object original, object target, object owner)
        {
            throw new NotImplementedException();
        }

        public object Assemble(object cached, object owner)
        {
            throw new NotImplementedException();
        }

        public object Disassemble(object value)
        {
            throw new NotImplementedException();
        }

        public SqlType[] SqlTypes { get; private set; }
        public Type ReturnedType { get { return typeof(UserValueType); } }
        public bool IsMutable { get; private set; }
    }
}
