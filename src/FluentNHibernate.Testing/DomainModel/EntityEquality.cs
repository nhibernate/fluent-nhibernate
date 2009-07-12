using FluentNHibernate.Data;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
	[TestFixture]
	public class EntityEquality
	{
		[Test]
		public void Two_entities_with_the_same_Id_should_equal_each_other()
		{
            var first = new ConcreteEntity { Id = 99 };
            var second = new ConcreteEntity { Id = 99 };

			first.Equals(second).ShouldBeTrue();
			second.Equals(first).ShouldBeTrue();


			Equals(first, second).ShouldBeTrue();
			Equals(second, first).ShouldBeTrue();

			first.GetHashCode().ShouldEqual(second.GetHashCode());

			(first == second).ShouldBeTrue();
			(second == first).ShouldBeTrue();

			(first != second).ShouldBeFalse();
			(second != first).ShouldBeFalse();
		}

		[Test]
		public void Two_entities_with_different_Ids_should_not_equal_each_other()
		{
            var first = new ConcreteEntity { Id = 66 };
            var second = new ConcreteEntity { Id = 77 };

			first.Equals(second).ShouldBeFalse();
			second.Equals(first).ShouldBeFalse();


			Equals(first, second).ShouldBeFalse();
			Equals(second, first).ShouldBeFalse();

			first.GetHashCode().ShouldNotEqual(second.GetHashCode());

			(first == second).ShouldBeFalse();
			(second == first).ShouldBeFalse();

			(first != second).ShouldBeTrue();
			(second != first).ShouldBeTrue();
		}

		[Test]
		public void Subclassed_entities_should_equal_each_other_with_same_Id()
		{
			var first = new TestSubEntity {Id = 99};
			var second = new TestSubEntity { Id = 99 };

			first.Equals(second).ShouldBeTrue();
		}

		[Test]
		public void Subclassed_entities_should_not_equal_entities_of_a_different_type_even_if_the_Id_is_the_same()
		{
			var first = new TestSubEntity { Id = 99 };
			var second = new AnotherSubEntity { Id = 99 };

			first.Equals(second).ShouldBeFalse();
		}

		[Test]
		public void Deep_subclassed_entities_should_not_equal_their_parent_classed_entities_even_if_the_Id_is_the_same()
		{
			var first = new TestSubEntity { Id = 99 };
			var second = new DeepSubEntity { Id = 99 };

			first.Equals(second).ShouldBeFalse();
		}

        public class ConcreteEntity : Entity
        {}

        public class TestSubEntity : ConcreteEntity
		{
		}

        public class AnotherSubEntity : ConcreteEntity
		{
		}

		public class DeepSubEntity : TestSubEntity
		{
		}
	}
}