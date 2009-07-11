using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values
{
	public abstract class PropertySpecification<T> : Specification where T : new()
	{
		private PropertyInfo property;
		protected T target;
		protected Property<T, string> sut;

		public override void establish_context()
		{
			property = ReflectionHelper.GetProperty(GetPropertyExpression());
			target = new T();

			sut = new Property<T, string>(property, "bar");
		}

		protected abstract Expression<Func<T, string>> GetPropertyExpression();
	}

	public class When_a_property_with_a_public_setter_is_set : PropertySpecification<PropertyEntity>
	{
		protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
		{
			return x => x.GetterAndSetter;
		}

		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_succeed()
		{
			target.GetterAndSetter.ShouldEqual("bar");
		}
	}

	public class When_a_property_with_a_private_setter_is_set : PropertySpecification<PropertyEntity>
	{
		protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
		{
			return x => x.GetterAndPrivateSetter;
		}

		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_succeed()
		{
			target.GetterAndPrivateSetter.ShouldEqual("bar");
		}
	}
	
	public class When_a_property_with_a_backing_field_is_set : PropertySpecification<PropertyEntity>
	{
		protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
		{
			return x => x.BackingField;
		}

		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_fail()
		{
			thrown_exception.ShouldBeOfType<ApplicationException>();
			thrown_exception.Message.ShouldStartWith("Error while trying to set property");
		}
	}

	public class When_a_property_is_set_with_a_custom_setter : PropertySpecification<PropertyEntity>
	{
		protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
		{
			return x => x.BackingField;
		}

		public override void establish_context()
		{
			base.establish_context();

			sut.ValueSetter = (entity, propertyInfo, value) => entity.SetBackingField(value);
		}

		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_succeed()
		{
			target.BackingField.ShouldEqual("bar");
		}
	}

	public class When_a_property_is_set_with_a_custom_setter_that_fails : PropertySpecification<PropertyEntity>
	{
		protected override Expression<Func<PropertyEntity, string>> GetPropertyExpression()
		{
			return x => x.BackingField;
		}

		public override void establish_context()
		{
			base.establish_context();

			sut.ValueSetter = (entity, propertyInfo, value) => { throw new Exception(); };
		}

		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_fail()
		{
			thrown_exception.ShouldBeOfType<ApplicationException>();
			thrown_exception.Message.ShouldStartWith("Error while trying to set property");
		}
	}
}