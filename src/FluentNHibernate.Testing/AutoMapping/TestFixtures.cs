using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Drawing;
using FluentNHibernate.Automapping.TestFixtures.ComponentTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomCompositeTypes;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Data.Common;

namespace FluentNHibernate.Automapping.TestFixtures
{
    public abstract class EntityBaseClassWithPrivateSetter
    {
        public int Id { get; private set; }
    }

    public class ClassThatInheritsIdFromParentWithPrivateSetter : EntityBaseClassWithPrivateSetter
    {

    }

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

	public class ExampleBaseClass
	{
		public virtual int Id { get; set; }
	}

	public class FirstInheritedClass : ExampleBaseClass
	{
		public string Property1 { get; set; }
		public int PropertyAlsoOnSiblingInheritedClass { get; set; }
	}

	public class SecondInheritedClass : ExampleBaseClass
	{
		public string Property2 { get; set; }
		public int PropertyAlsoOnSiblingInheritedClass { get; set; }
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
        public virtual int Id { get; private set; }
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
        public DoubleString SomeStringTuple { get; set; }
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
        public IList<Customer> Residents { get; set; }
    }
}

namespace FluentNHibernate.Automapping.TestFixtures.CustomTypes
{
    public class Custom
    {

    }

    public class DoubleString
    {
        public string s1 { get; set; }
        public string s2 { get; set; }
    }

    public class CustomTypeConvention : UserTypeConvention<CustomUserType>
    {}

    public class CustomCompositeTypeConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
           criteria.Expect(x => x.Type == typeof(DoubleString));
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType<DoubleStringType>();
        }
    }

    public class CompositeTypeConventionWithCustomPrefix : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
           criteria.Expect(x => x.Type == typeof(DoubleString));
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType<DoubleStringType>(instance.Property.PropertyType.Name + "WithCustomPrefix_");
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

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            throw new NotImplementedException();
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            throw new NotImplementedException();
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
            if (x is null || y is null) return false;

            DoubleString lhs = (DoubleString)x;
            DoubleString rhs = (DoubleString)y;

            return lhs.s1 == rhs.s1 && lhs.s2 == rhs.s2;
        }

        public int GetHashCode(object x)
        {
            unchecked
            {
                DoubleString a = (DoubleString)x;
                return a.s1.GetHashCode() + 31 * a.s2.GetHashCode();
            }
        }

        public Object DeepCopy(Object x)
        {
            if (x is null) return null;
            DoubleString result = new DoubleString();
            DoubleString input = (DoubleString)x;
            result.s1 = input.s1;
            result.s2 = input.s2;
            return result;
        }

        public bool IsMutable
        {
            get { return true; }
        }

        public object NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
        {
            string first = (string)NHibernateUtil.String.NullSafeGet(dr, names[0], session, owner);
            string second = (string)NHibernateUtil.String.NullSafeGet(dr, names[1], session, owner);

            return (first is null && second is null) ? null : new string[] { first, second };
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
        {
            DoubleString ds = value as DoubleString ?? new DoubleString();

            NHibernateUtil.String.NullSafeSet(cmd, ds.s1, index, session);
            NHibernateUtil.String.NullSafeSet(cmd, ds.s2, index + 1, session);
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

    public abstract class Base1
    {
        public virtual int Id { get; protected set; }
        public abstract void Foo(int x);
    }

    public class Derived1 : Base1
    {
        public virtual Decimal Rate { get; set; }
        public override void Foo(int x)
        {
        }

        public string GetSomething()
        {
            return Environment.NewLine;
        }
    }

    public class SecondLevel : Derived1
    {
        public virtual Double SecondRate { get; set; }
    }

    public class ThirdLevel : SecondLevel
    {
        public virtual Boolean Flag { get; set; }
        public virtual TimeSpan Version { get; set; }
    }

    public class FourthLevel: ThirdLevel
    {
        public PublisherType publisherType { get; set; }
        public ToOne One { get; set; }
        public IList<ManyToMany> Many { get; set; }
    }

    public class ToOne
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
    }

    public class ManyToMany
    {
        public virtual int Id { get; protected set; }
        public virtual Decimal Value { get; set; }
        public IList<FourthLevel> Levels { get; set; }
    }

    public enum PublisherType
    {
        Online,
        Offline,
        Mixed
    }
}
