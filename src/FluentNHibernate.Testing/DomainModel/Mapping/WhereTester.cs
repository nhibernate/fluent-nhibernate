using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
	[TestFixture]
	public class WhereTester
	{
		[Test]
		public void ShouldAddWhereAttributeToClass()
		{
			new MappingTester<MappedObject>()
				.ForMapping(x => x.Where("deleted=0"))
				.Element("class")
				.HasAttribute("where", "deleted=0");
		}
	}
}