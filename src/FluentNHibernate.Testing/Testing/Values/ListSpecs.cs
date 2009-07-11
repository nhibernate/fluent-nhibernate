using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values
{
	public abstract class With_list_entity : Specification
	{
		private PropertyInfo property;
		protected ListEntity target;
		protected List<ListEntity, string> sut;
		protected string[] listItems;

		public override void establish_context()
		{
			property = ReflectionHelper.GetProperty(GetPropertyExpression());
			target = new ListEntity();

			listItems = new[] {"foo", "bar", "baz"};
			sut = new List<ListEntity, string>(property, listItems);
		}

		protected abstract Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression();
	}

	public abstract class When_a_list_property_with_is_set_successfully : With_list_entity
	{
		public override void because()
		{
			sut.SetValue(target);
		}

		[Test]
		public void should_succeed()
		{
			thrown_exception.ShouldBeNull();
		}

		[Test]
		public abstract void should_set_the_list_items();
	}

	public class When_a_list_property_with_a_public_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.GetterAndSetter;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.GetterAndSetter.ShouldContain(listItem);
			}
		}
	}

	public class When_a_list_property_with_a_private_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.GetterAndPrivateSetter;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.GetterAndPrivateSetter.ShouldContain(listItem);
			}
		}
	}

	public class When_a_list_property_is_set_with_a_custom_setter : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.BackingField;
		}

		public override void establish_context()
		{
			base.establish_context();

			sut.ValueSetter = (entity, propertyInfo, value) =>
			{
				foreach (var listItem in value)
				{
					entity.AddListItem(listItem);
				}
			};
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.BackingField.ShouldContain(listItem);
			}
		}
	}

	public class When_a_set_property_with_a_public_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.Set;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.Set.Contains(listItem).ShouldBeTrue();
			}
		}
	}

	public class When_a_typed_set_property_with_a_public_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.TypedSet;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.TypedSet.ShouldContain(listItem);
			}
		}
	}

	public class When_a_collection_property_with_a_public_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.Collection;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			string[] array = new string[target.Collection.Count];
			target.Collection.CopyTo(array, 0);

			foreach (var listItem in listItems)
			{
				array.ShouldContain(listItem);
			}
		}
	}

	public class When_an_array_property_with_a_public_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.Array;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.Array.ShouldContain(listItem);
			}
		}
	}
	
	public class When_an_list_property_with_a_public_setter_is_set : When_a_list_property_with_is_set_successfully
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
		{
			return x => x.List;
		}

		[Test]
		public override void should_set_the_list_items()
		{
			foreach (var listItem in listItems)
			{
				target.List.ShouldContain(listItem);
			}
		}
	}

	public class When_a_list_property_with_a_backing_field_is_set : With_list_entity
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
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
		}

		[Test]
		public void should_tell_which_property_failed_to_be_set()
		{
			var exception = (ApplicationException)thrown_exception;
			exception.Message.ShouldEqual("Error while trying to set property BackingField");
		}
	}

	public class When_a_list_property_is_set_with_a_custom_setter_that_fails : With_list_entity
	{
		protected override Expression<Func<ListEntity, IEnumerable>> GetPropertyExpression()
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
		}

		[Test]
		public void should_tell_which_property_failed_to_be_set()
		{
			var exception = (ApplicationException)thrown_exception;
			exception.Message.ShouldEqual("Error while trying to set property BackingField");
		}
	}
}