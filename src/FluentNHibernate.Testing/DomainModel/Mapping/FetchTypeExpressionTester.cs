using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
	[TestFixture]
	public class FetchTypeExpressionTester
	{
		#region Test Setup
		public FetchTypeExpression<IMappingPart> _fetchType;
		public Cache<string, string> _properties;
		
		[SetUp]
		public virtual void SetUp()
		{
			_properties = new Cache<string, string>();
			_fetchType = new FetchTypeExpression<IMappingPart>(null, _properties);
		}

		protected FetchTypeExpressionTester A_call_to(Func<IMappingPart> fetchAction)
		{
			fetchAction();
			return this;
		}

		private void should_set_the_fetch_value_to(string expected)
		{
			_properties.Get("fetch").ShouldEqual(expected);
		}

		#endregion

		[Test]
		public void Join_should_add_the_correct_fetch_attribute_to_the_parent_part()
		{
			A_call_to(_fetchType.Join).should_set_the_fetch_value_to("join");
		}

		[Test]
		public void Select_should_add_the_correct_fetch_attribute_to_the_parent_part()
		{
			A_call_to(_fetchType.Select).should_set_the_fetch_value_to("select");
		}

	}
}
