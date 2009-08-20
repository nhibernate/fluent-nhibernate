using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Drawing;
using FluentNHibernate.Automapping.TestFixtures.ComponentTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomCompositeTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace FluentNHibernate.Automapping.TestFixtures
{
    public abstract class EntityBase<TPK>
    {
        public virtual TPK Id { get; set; }
    }

    public class ClassUsingGenericBase : EntityBase<Guid>
    {
        public virtual string Name { get; set; }
    }

    public class ExampleCustomColumn
    {
        public int Id { get; set; }
        public int ExampleCustomColumnId { get; set; }
        public int CustomColumn
        {
            get
            {
                return 12;
            }
        }
    }

    public class ExampleInheritedClass : ExampleClass
    {
        public string ExampleProperty { get; set; }
        public int SomeNumber{ get; set; }
        public IList<ExampleClass> Children { get; private set; }
        public ExampleParentClass Component { get; set; }
        public IDictionary DictionaryChild { get; set; }
    }

    public class ClassWithDummyProperty
    {
        public virtual int Id { get; set; }
        public virtual string Dummy { get; set; }
        public virtual string Dummy1 { get; set; }
        public virtual string Dummy2 { get; set; }
    }

    public class AnotherClassWithDummyProperty
    {
        public virtual int Id { get; set; }
        public virtual string Dummy { get; set; }
        public virtual string Dummy1 { get; set; }
        public virtual string Dummy2 { get; set; }
    }

    public class ExampleClass
    {
        public virtual int Id { get; set; }
        public virtual int ExampleClassId { get; set; }
        public virtual string LineOne { get; set; }
        public TimeSpan Timestamp { get; set; }
        public ExampleEnum Enum { get; set; }
        public ExampleParentClass Parent { get; set; }
        public IDictionary Dictionary { get; set; }
    }

    public class PrivateIdSetterClass
    {
        private int id;

        public virtual int Id
        {
            get { return id; }
        }
    }


    public enum ExampleEnum
    {
        enum1=1
    }

    public class ExampleParentClass
    {
        public int ExampleParentClassId { get; set; } 
        public virtual int Id { get; set; }
        public virtual IList<ExampleClass> Examples {get; set;}
    }


    public class ValidTimestampClass
    {
        public virtual int Id { get; set; }
        public virtual int Timestamp { get; set; }
    }

    public class ValidVersionClass
    {
        public virtual int Id { get; set; }
        public virtual long Version { get; set; }
    }

    public class InvalidTimestampClass
    {
        public virtual int Id { get; set; }
        public virtual DateTime Timestamp { get; set; }
    }

    public class InvalidVersionClass
    {
        public virtual int Id { get; set; }
        public virtual string Version { get; set; }
    }

    public class ClassWithUserType
    {
        public int Id { get; set; }
        public Custom Custom { get; set; }
    }

    public class ClassWithCompositeUserType
    {
        public int Id { get; set; }
        public DoubleStringType DoubleStringType { get; set; }
    }

    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual Address WorkAddress { get; set; }
    }

    public class ClassWithBitmap
    {
        public virtual int Id { get; set; }
        public virtual Bitmap Bitmap { get; set; }
    }

    public class ClassWithGuidId
    {
        public virtual Guid Id { get; set; }
    }

    public class ClassWithIntId
    {
        public virtual int Id { get; set; }
    }

    public class ClassWithLongId
    {
        public virtual long Id { get; set; }
    }

    public class ClassWithStringId
    {
        public virtual string Id { get; set; }
    }
}

namespace FluentNHibernate.Automapping.TestFixtures.ComponentTypes
{
    public class Address
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public Custom Custom { get; set; }
    }
}

namespace FluentNHibernate.Automapping.TestFixtures.CustomTypes
{
    public class Custom
    {

    }

    public class CustomTypeConvention : UserTypeConvention<CustomUserType>
    {}

    public class CustomUserType : IUserType
    {
        public bool Equals(object x, object y)
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
            get { return new[] {new SqlType(DbType.String)}; }
        }

        public Type ReturnedType
        {
            get { return typeof(Custom); }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }
}

namespace FluentNHibernate.Automapping.TestFixtures.CustomCompositeTypes
{
    public class DoubleStringType : ICompositeUserType
    {
        public System.Type ReturnedClass
        {
            get { return typeof(string[]); }
        }

        public new bool Equals(object x, object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            string[] lhs = (string[])x;
            string[] rhs = (string[])y;

            return lhs[0].Equals(rhs[0]) && lhs[1].Equals(rhs[1]);
        }

        public int GetHashCode(object x)
        {
            unchecked
            {
                string[] a = (string[])x;
                return a[0].GetHashCode() + 31 * a[1].GetHashCode();
            }
        }

        public Object DeepCopy(Object x)
        {
            if (x == null) return null;
            string[] result = new string[2];
            string[] input = (string[])x;
            result[0] = input[0];
            result[1] = input[1];
            return result;
        }

        public bool IsMutable
        {
            get { return true; }
        }

        public Object NullSafeGet(IDataReader rs, string[] names, ISessionImplementor session, Object owner)
        {
            string first = (string)NHibernateUtil.String.NullSafeGet(rs, names[0], session, owner);
            string second = (string)NHibernateUtil.String.NullSafeGet(rs, names[1], session, owner);

            return (first == null && second == null) ? null : new string[] { first, second };
        }


        public void NullSafeSet(IDbCommand st, Object value, int index, ISessionImplementor session)
        {
            string[] strings = (value == null) ? new string[2] : (string[])value;

            NHibernateUtil.String.NullSafeSet(st, strings[0], index, session);
            NHibernateUtil.String.NullSafeSet(st, strings[1], index + 1, session);
        }

        public string[] PropertyNames
        {
            get { return new string[] { "s1", "s2" }; }
        }

        public IType[] PropertyTypes
        {
            get { return new IType[] { NHibernateUtil.String, NHibernateUtil.String }; }
        }

        public Object GetPropertyValue(Object component, int property)
        {
            return ((string[])component)[property];
        }

        public void SetPropertyValue(
                Object component,
                int property,
                Object value)
        {
            ((string[])component)[property] = (string)value;
        }

        public object Assemble(
                object cached,
                ISessionImplementor session,
                object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(Object value, ISessionImplementor session)
        {
            return DeepCopy(value);
        }

        public object Replace(object original, object target, ISessionImplementor session, object owner)
        {
            return DeepCopy(original);
        }
    }

    /// <summary>
    /// Extracted from the NHibernate.Model
    /// </summary>
    public class CustomCompositeUserType : ICompositeUserType
    {
        public System.Type ReturnedClass
        {
            get { return typeof(string[]); }
        }

        public new bool Equals(object x, object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            string[] lhs = (string[])x;
            string[] rhs = (string[])y;

            return lhs[0].Equals(rhs[0]) && lhs[1].Equals(rhs[1]);
        }

        public int GetHashCode(object x)
        {
            unchecked
            {
                string[] a = (string[])x;
                return a[0].GetHashCode() + 31 * a[1].GetHashCode();
            }
        }

        public Object DeepCopy(Object x)
        {
            if (x == null) return null;
            string[] result = new string[2];
            string[] input = (string[])x;
            result[0] = input[0];
            result[1] = input[1];
            return result;
        }

        public bool IsMutable
        {
            get { return true; }
        }

        public Object NullSafeGet(IDataReader rs, string[] names, ISessionImplementor session, Object owner)
        {
            string first = (string)NHibernateUtil.String.NullSafeGet(rs, names[0], session, owner);
            string second = (string)NHibernateUtil.String.NullSafeGet(rs, names[1], session, owner);

            return (first == null && second == null) ? null : new string[] { first, second };
        }

        public void NullSafeSet(IDbCommand st, Object value, int index, ISessionImplementor session)
        {
            string[] strings = (value == null) ? new string[2] : (string[])value;

            NHibernateUtil.String.NullSafeSet(st, strings[0], index, session);
            NHibernateUtil.String.NullSafeSet(st, strings[1], index + 1, session);
        }

        public string[] PropertyNames
        {
            get { return new string[] { "s1", "s2" }; }
        }

        public IType[] PropertyTypes
        {
            get { return new IType[] { NHibernateUtil.String, NHibernateUtil.String }; }
        }

        public Object GetPropertyValue(Object component, int property)
        {
            return ((string[])component)[property];
        }

        public void SetPropertyValue(Object component, int property, Object value)
        {
            ((string[])component)[property] = (string)value;
        }

        public object Assemble(object cached, ISessionImplementor session, object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(Object value, ISessionImplementor session)
        {
            return DeepCopy(value);
        }

        public object Replace(object original, object target, ISessionImplementor session, object owner)
        {
            return DeepCopy(original);
        }
    }

}

namespace FluentNHibernate.Automapping.TestFixtures.SuperTypes
{
    public class SuperType
    {
        public int Id { get; set; }
    }

    public class ExampleCustomColumn : SuperType
    {
        public int ExampleCustomColumnId { get; set; }
        public int CustomColumn
        {
            get
            {
                return 12;
            }
        }
    }

    public class ExampleInheritedClass : ExampleClass
    {
        public string ExampleProperty { get; set; }
        public int SomeNumber{ get; set; }
    }


    public class ExampleClass : SuperType
    {
        public virtual int ExampleClassId { get; set; }
        public virtual string LineOne { get; set; }
        public DateTime Timestamp { get; set; }
        public ExampleEnum Enum { get; set; }
        public ExampleParentClass Parent { get; set; }
    }


    public enum ExampleEnum
    {
        enum1=1
    }

    public class ExampleParentClass : SuperType
    {
        public int ExampleParentClassId { get; set; } 
        public virtual IList<ExampleClass> Examples {get; set;}
    }
}
