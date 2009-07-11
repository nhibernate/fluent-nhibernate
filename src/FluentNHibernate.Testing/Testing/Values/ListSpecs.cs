using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values
{
	public abstract class ListSpecification<T> : Specification where T : new()
	{
		private PropertyInfo property;
		protected T target;
		protected List<T, string> sut;
		protected List<string> listItems;

		public override void establish_context()
		{
			property = ReflectionHelper.GetProperty(GetPropertyExpression());
			target = new T();

			listItems = new List<string>{"foo", "bar", "baz"};
			sut = new List<T, string>(property, listItems);
		}

		protected abstract Expression<Func<T, IEnumerable<string>>> GetPropertyExpression();
	}

	public class When_a_list_property_with_a_public_setter_is_set : ListSpecification<ListEntity>
	{
		protected override Expression<Func<ListEntity, IEnumerable<string>>> GetPropertyExpression()
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
			foreach (var listItem in listItems)
			{
				target.GetterAndSetter.ShouldContain(listItem);
			}
		}
	}

	public class When_a_list_property_with_a_private_setter_is_set : ListSpecification<ListEntity>
	{
		protected override Expression<Func<ListEntity, IEnumerable<string>>> GetPropertyExpression()
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
			foreach (var listItem in listItems)
			{
				target.GetterAndPrivateSetter.ShouldContain(listItem);
			}
		}
	}

	public class When_a_list_property_with_a_backing_field_is_set : ListSpecification<ListEntity>
	{
		protected override Expression<Func<ListEntity, IEnumerable<string>>> GetPropertyExpression()
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

	public class When_a_list_property_is_set_with_a_custom_setter : ListSpecification<ListEntity>
	{
		protected override Expression<Func<ListEntity, IEnumerable<string>>> GetPropertyExpression()
		{
			return x => x.BackingField;
		}

		public override void establish_context()
		{
			base.establish_context();

			sut.ValueSetter = (entity, propertyInfo, value) =>
			{
				foreach (var listItem in (IEnumerable<string>)value)
				{
					entity.AddListItem(listItem);	
				}
			};
		}

		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_succeed()
		{
			foreach (var listItem in listItems)
			{
				target.BackingField.ShouldContain(listItem);
			}
		}
	}

	public class When_a_list_property_is_set_with_a_custom_setter_that_fails : ListSpecification<ListEntity>
	{
		protected override Expression<Func<ListEntity, IEnumerable<string>>> GetPropertyExpression()
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