using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Equality
{
    [TestFixture]
    public class when_comparing_the_contents_of_two_different_IEnumerables_with_the_same_items : IEnumerableContentEqualitySpec
    {
        public override void establish_context()
        {
            first_enumerable = new List<string> { "one", "two" };
            second_enumerable = new List<string> { "one", "two" };
        }

        public override void because()
        {
            are_equal = first_enumerable.ContentEquals(second_enumerable);
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_the_contents_of_two_different_IEnumerables_with_different_items : IEnumerableContentEqualitySpec
    {
        public override void establish_context()
        {
            first_enumerable = new List<string> { "one", "two" };
            second_enumerable = new List<string> { "one", "two", "three" };
        }

        public override void because()
        {
            are_equal = first_enumerable.ContentEquals(second_enumerable);
        }

        [Test]
        public void should_not_be_equal()
        {
            are_equal.ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_comparing_the_contents_of_two_different_IEnumerables_with_same_items_in_a_different_order : IEnumerableContentEqualitySpec
    {
        public override void establish_context()
        {
            first_enumerable = new List<string> { "one", "two" };
            second_enumerable = new List<string> { "two", "one" };
        }

        public override void because()
        {
            are_equal = first_enumerable.ContentEquals(second_enumerable);
        }

        [Test]
        public void should_not_be_equal()
        {
            are_equal.ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_comparing_the_contents_of_two_empty_IEnumerables : IEnumerableContentEqualitySpec
    {
        public override void establish_context()
        {
            first_enumerable = new List<string>();
            second_enumerable = new List<string>();
        }

        public override void because()
        {
            are_equal = first_enumerable.ContentEquals(second_enumerable);
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_the_contents_of_two_diferent_IDictionarys_with_the_same_contents : IDictionaryContentEqualitySpec
    {
        public override void establish_context()
        {
            first_dictionary = new Dictionary<string, string> { { "one", "one-val" }, { "two", "two-val" } };
            second_dictionary = new Dictionary<string, string> { { "one", "one-val" }, { "two", "two-val" } };
        }

        public override void because()
        {
            are_equal = first_dictionary.ContentEquals(second_dictionary);
        }

        [Test]
        public void should_be_equal()
        {
            are_equal.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_comparing_the_contents_of_two_diferent_IDictionarys_with_different_contents : IDictionaryContentEqualitySpec
    {
        public override void establish_context()
        {
            first_dictionary = new Dictionary<string, string> { { "one", "one-val" }, { "two", "two-val" } };
            second_dictionary = new Dictionary<string, string> { { "one", "val-one" }, { "two", "val-two" } };
        }

        public override void because()
        {
            are_equal = first_dictionary.ContentEquals(second_dictionary);
        }

        [Test]
        public void should_not_be_equal()
        {
            are_equal.ShouldBeFalse();
        }
    }

    #region spec bases

    public abstract class IEnumerableContentEqualitySpec : Specification
    {
        protected IEnumerable<string> first_enumerable;
        protected IEnumerable<string> second_enumerable;
        protected bool are_equal;
    }

    public abstract class IDictionaryContentEqualitySpec : Specification
    {
        protected IDictionary<string, string> first_dictionary;
        protected IDictionary<string, string> second_dictionary;
        protected bool are_equal;
    }

    #endregion
}