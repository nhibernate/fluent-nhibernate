using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Testing.Values
{
    public abstract class With_list_entity : Specification
    {
        private Accessor property;
        protected ListEntity target;
        protected List<ListEntity, string> sut;
        protected string[] listItems;

        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor (GetPropertyExpression ());
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
        public abstract void should_set_the_list_items();

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    [TestFixture]
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

    public abstract class With_initialized_list : Specification
    {
        protected Accessor property;
        protected ListEntity target;
        protected List<ListEntity, string> sut;

        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable<string>>>)(x => x.GetterAndSetter));
            target = new ListEntity();

            sut = new List<ListEntity, string>(property, new[] {"foo", "bar", "baz"});
        }
    }

    [TestFixture]
    public class When_the_checked_list_is_equal_to_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            base.establish_context();
            target.GetterAndSetter = new[] {"foo", "bar", "baz"};
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }

    [TestFixture]
    public class When_the_checked_list_has_transposed_items_of_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            base.establish_context();
            target.GetterAndSetter = new[] {"baz", "bar", "foo"};
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_fail()
        {
            thrown_exception.ShouldBeOfType<ApplicationException>();
        }

        [Test]
        public void should_tell_that_the_list_element_count_does_not_match()
        {
            var exception = (ApplicationException)thrown_exception;
            exception.Message.ShouldEqual("Expected 'foo' but got 'baz' at position 0");
        }
    }

    [TestFixture]
    public class When_the_checked_list_does_not_have_the_same_number_of_elements_as_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            base.establish_context();
            target.GetterAndSetter = new[] {"foo"};
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_fail()
        {
            thrown_exception.ShouldBeOfType<ApplicationException>();
        }

        [Test]
        public void should_tell_that_the_list_element_count_does_not_match()
        {
            var exception = (ApplicationException)thrown_exception;
            exception.Message.ShouldEqual("Actual count (1) does not equal expected count (3)");
        }
    }

    [TestFixture]
    public class When_the_checked_list_is_null : With_initialized_list
    {
        public override void establish_context()
        {
            base.establish_context();
            target.GetterAndSetter = null;
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_fail()
        {
            thrown_exception.ShouldBeOfType<ArgumentNullException>();
        }

        [Test]
        public void should_tell_that_actual_list_is_null()
        {
            var exception = (ArgumentNullException)thrown_exception;
            exception.Message.ShouldStartWith("Actual and expected are not equal (actual was null).");
        }
    }

    [TestFixture]
    public class When_the_expected_list_is_null : With_initialized_list
    {
        public override void establish_context()
        {
            base.establish_context();
            sut = new List<ListEntity, string>(property, null);
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_fail()
        {
            thrown_exception.ShouldBeOfType<ArgumentNullException>();
        }

        [Test]
        public void should_tell_that_actual_list_is_null()
        {
            var exception = (ArgumentNullException)thrown_exception;
            exception.Message.ShouldStartWith("Actual and expected are not equal (expected was null).");
        }
    }

    public class When_the_checked_list_is_equal_to_the_expected_list_with_a_custom_equality_comparer : When_the_checked_list_is_equal_to_the_expected_list
    {
        public override void establish_context()
        {
            base.establish_context();

            sut.EntityEqualityComparer = MockRepository.GenerateStub<IEqualityComparer>();
            sut.EntityEqualityComparer
                .Stub(x => x.Equals(null, null))
                .IgnoreArguments()
                .Return(true);
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_perform_the_check_with_the_custom_equality_comparer()
        {
            sut.EntityEqualityComparer.AssertWasCalled(x => x.Equals(null, null),
                o => o.IgnoreArguments().Repeat.Times(3));
        }
    }

    public class When_the_checked_list_has_transposed_items_of_the_expected_list_with_a_custom_equality_comparer : When_the_checked_list_has_transposed_items_of_the_expected_list
    {
        public override void establish_context()
        {
            base.establish_context();

            sut.EntityEqualityComparer = MockRepository.GenerateStub<IEqualityComparer>();
            sut.EntityEqualityComparer
                .Stub(x => x.Equals(null, null))
                .IgnoreArguments()
                .Return(false);
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_perform_the_check_with_the_custom_equality_comparer()
        {
            sut.EntityEqualityComparer.AssertWasCalled(x => x.Equals(null, null),
                o => o.IgnoreArguments().Repeat.Once());
        }
    }

    [TestFixture]
    public class When_a_list_is_checked_with_a_custom_equality_comparer_that_fails : With_initialized_list
    {
        private InvalidOperationException exception;

        public override void establish_context()
        {
            base.establish_context();
            target.GetterAndSetter = new[] {"foo", "bar", "baz"};

            exception = new InvalidOperationException();

            sut.EntityEqualityComparer = MockRepository.GenerateStub<IEqualityComparer>();
            sut.EntityEqualityComparer
                .Stub(x => x.Equals(null, null))
                .IgnoreArguments()
                .Throw(exception);
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_fail_with_the_exception_from_the_equality_comparer()
        {
            thrown_exception.ShouldBeTheSameAs(exception);
        }
    }

    [TestFixture]
    public class When_the_checked_typed_set_is_equal_to_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable<string>>>)(x => x.TypedSet));
            target = new ListEntity();

            sut = new List<ListEntity, string>(property, new[] {"foo", "bar", "baz"});

            target.TypedSet.AddAll(new[] {"foo", "bar", "baz"});
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }

    [TestFixture]
    public class When_the_checked_set_is_equal_to_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable>>)(x => x.Set));
            target = new ListEntity();

            sut = new List<ListEntity, string>(property, new[] {"foo", "bar", "baz"});

            target.Set.AddAll(new[] {"foo", "bar", "baz"});
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }

    [TestFixture]
    public class When_the_collection_is_equal_to_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable>>)(x => x.Collection));
            target = new ListEntity();

            sut = new List<ListEntity, string>(property, new[] {"foo", "bar", "baz"});

            target.Collection = new List<string> {"foo", "bar", "baz"};
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }

    [TestFixture]
    public class When_the_typed_list_is_equal_to_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable>>)(x => x.List));
            target = new ListEntity();

            sut = new List<ListEntity, string>(property, new[] {"foo", "bar", "baz"});

            target.List = new List<string> {"foo", "bar", "baz"};
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }

    [TestFixture]
    public class When_the_checked_array_is_equal_to_the_expected_list : With_initialized_list
    {
        public override void establish_context()
        {
            property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable<string>>>)(x => x.Array));
            target = new ListEntity();

            sut = new List<ListEntity, string>(property, new[] {"foo", "bar", "baz"});

            target.Array = new[] {"foo", "bar", "baz"};
        }

        public override void because()
        {
            sut.CheckValue(target);
        }

        [Test]
        public void should_succeed()
        {
            thrown_exception.ShouldBeNull();
        }
    }
}