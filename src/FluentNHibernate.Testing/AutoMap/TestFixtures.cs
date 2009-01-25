using System.Collections.Generic;
using System;
using System.Data;
using FluentNHibernate.AutoMap.TestFixtures.ComponentTypes;
using FluentNHibernate.AutoMap.TestFixtures.CustomTypes;
using FluentNHibernate.Mapping;
using Iesi.Collections.Generic;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace FluentNHibernate.AutoMap.TestFixtures
{
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
    }


    public class ExampleClass
    {
        public virtual int Id { get; set; }
        public virtual int ExampleClassId { get; set; }
        public virtual string LineOne { get; set; }
        public TimeSpan Timestamp { get; set; }
        public ExampleEnum Enum { get; set; }
        public ExampleParentClass Parent { get; set; }
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

    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual Address WorkAddress { get; set; }
    }
}

namespace FluentNHibernate.AutoMap.TestFixtures.ComponentTypes
{
    public class Address
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public Custom Custom { get; set; }
    }
}

namespace FluentNHibernate.AutoMap.TestFixtures.CustomTypes
{
    public class Custom
    {

    }

    public class CustomTypeConvention : ITypeConvention
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(Custom);
        }

        public void AlterMap(IProperty propertyMapping)
        {
            propertyMapping.CustomTypeIs<CustomUserType>();
        }
    }

    public class CustomUserType : IUserType
    {
        public bool Equals(object x, object y)
        {
            throw new System.NotImplementedException();
        }

        public int GetHashCode(object x)
        {
            throw new System.NotImplementedException();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            throw new System.NotImplementedException();
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            throw new System.NotImplementedException();
        }

        public object DeepCopy(object value)
        {
            throw new System.NotImplementedException();
        }

        public object Replace(object original, object target, object owner)
        {
            throw new System.NotImplementedException();
        }

        public object Assemble(object cached, object owner)
        {
            throw new System.NotImplementedException();
        }

        public object Disassemble(object value)
        {
            throw new System.NotImplementedException();
        }

        public SqlType[] SqlTypes
        {
            get { throw new System.NotImplementedException(); }
        }
        public Type ReturnedType
        {
            get { throw new System.NotImplementedException(); }
        }
        public bool IsMutable
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}

namespace FluentNHibernate.AutoMap.TestFixtures.SuperTypes
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
