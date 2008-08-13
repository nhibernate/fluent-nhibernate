using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
	[TestFixture]
	public class CompositeIdentityPartTester
	{
		[Test]
		public void Defaults()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId().WithKeyProperty(x => x.LongId))
				.Element("class/composite-id/key-property")
					.HasAttribute("name", "LongId");
		}

		[Test]
		public void Key_property_Explicit_column_name()
		{

			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId().WithKeyProperty(x => x.LongId, "SomeColumn"))
				.Element("class/composite-id/key-property")
					.HasAttribute("column", "SomeColumn");
		}

		[Test]
		public void Key_property_TypeIsSetToTypeName()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId().WithKeyProperty(x => x.LongId))
				.Element("class/composite-id/key-property")
					.HasAttribute("type", "Int64");
		}

		[Test]
		public void Key_property_TypeIsSetToFullTypeNameIfTypeGeneric()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId().WithKeyProperty(x => x.NullableLongId))
				.Element("class/composite-id/key-property")
					.HasAttribute("type", typeof(long?).FullName);
		}

		[Test]
		public void Key_many_to_one_Defaults()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId().WithKeyReference(x => x.Child))
				.Element("class/composite-id/key-many-to-one")
					.HasAttribute("name", "Child")
					.HasAttribute("class", typeof(CompIdChild).AssemblyQualifiedName);
		}

		[Test]
		public void Key_many_to_one_Explicit_column_name()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId().WithKeyReference(x => x.Child, "SomeColumn"))
				.Element("class/composite-id/key-many-to-one")
					.HasAttribute("column", "SomeColumn");
		}

		[Test]
		public void Mixed_key_property_and_many_to_one()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.UseCompositeId()
					.WithKeyProperty(x=>x.LongId)
					.WithKeyReference(x => x.Child))
				.Element("class/composite-id/key-property")
					.HasAttribute("name", "LongId")
				.RootElement.Element("class/composite-id/key-many-to-one")
					.HasAttribute("name", "Child");
		}


		public class CompIdTarget
		{
			public virtual long LongId { get; set; }
			public virtual long? NullableLongId { get; set; }
			public virtual CompIdChild Child { get; set; }

		}

		public class CompIdChild
		{
			public virtual long ChildId { get; set; }
		}
	}
}
