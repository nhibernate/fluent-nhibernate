using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing.Values
{
    public class ReferenceBagSpec
    {
        public abstract class With_initialized_bag : Specification
        {
            protected Accessor property;
            protected ListEntity target;
            protected ReferenceBag<ListEntity, string> sut;

            public override void establish_context()
            {
                property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable<string>>>)(x => x.GetterAndSetter));
                target = new ListEntity();

                sut = new ReferenceBag<ListEntity, string>(property, new[] { "foo", "bar", "baz" });
            }
        }

        [TestFixture]
        public class When_the_checked_bag_has_multiple_items : With_initialized_bag
        {
            public override void establish_context()
            {
                var baz = "baz";
                property = ReflectionHelper.GetAccessor((Expression<Func<ListEntity, IEnumerable<string>>>)(x => x.GetterAndSetter));
                target = new ListEntity();
                sut = new ReferenceBag<ListEntity, string>(property, new[] {"foo", baz, baz, "bar"});
                
                target.GetterAndSetter = new[] {baz, baz, "bar", "foo"};
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
        public class when_checked_bag_has_less_items : With_initialized_bag
        {
            public override void establish_context()
            {
                base.establish_context();
                target.GetterAndSetter = new[] {"foo", "bar"};
            }

            public override void because()
            {
                sut.CheckValue(target);
            }

            [Test]
            public void should_throw_exception()
            {
                thrown_exception.ShouldBeOfType(typeof(ApplicationException));
            }

            [Test]
            public void should_state_that_bag_doesnt_match()
            {
                var exception = (ApplicationException)thrown_exception;
                exception.Message.ShouldEqual("Actual count (2) does not equal expected count (3)");
            }
        }

        [TestFixture]
        public class when_the_checked_list_has_different_elements : With_initialized_bag
        {
            public override void establish_context()
            {
                base.establish_context();
                target.GetterAndSetter = new[] {"bad", "bar", "foo"};
            }

            public override void because()
            {
                sut.CheckValue(target);
            }

            [Test]
            public void should_throw_exception()
            {
                thrown_exception.ShouldBeOfType(typeof(ApplicationException));
            }

            [Test]
            public void should_state_that_bag_doesnt_match()
            {
                var exception = (ApplicationException)thrown_exception;
                exception.Message.ShouldEqual("Actual count of item bad (1) does not equal expected item count (0)");
            }
        }

        [TestFixture]
        public class When_the_checked_list_has_transposed_items_of_the_expected_list : With_initialized_bag
        {
            public override void establish_context()
            {
                base.establish_context();
                target.GetterAndSetter = new[] { "baz", "bar", "foo" };
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
}