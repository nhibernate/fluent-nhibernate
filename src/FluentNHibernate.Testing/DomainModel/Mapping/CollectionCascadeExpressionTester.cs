using System;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
	[TestFixture]
	public class CollectionCascadeExpressionTester : CascadeExpressionTester
	{
		private CollectionCascadeExpression<IMappingPart> _collectionCascade;
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			_cascade = _collectionCascade = new CollectionCascadeExpression<IMappingPart>(_mockPart);
		}
	
		[Test]
		public void AllDeleteOrphan_should_correct_add_the_cascade_attribute_to_the_parent_part()
		{
			A_call_to(_collectionCascade.AllDeleteOrphan).should_set_the_cascade_value_to("all-delete-orphan");
		}
	}
}
