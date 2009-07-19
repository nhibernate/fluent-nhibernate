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
				.ForMapping(c => c.CompositeId().KeyProperty(x => x.LongId))
				.Element("class/composite-id/key-property")
					.HasAttribute("name", "LongId");
		}

		[Test]
		public void KeyPropertyExplicitColumnName()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.CompositeId().KeyProperty(x => x.LongId, "SomeColumn"))
				.Element("class/composite-id/key-property/column")
					.HasAttribute("name", "SomeColumn");
		}

		[Test]
		public void KeyPropertyTypeIsSetToTypeName()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.CompositeId().KeyProperty(x => x.LongId))
				.Element("class/composite-id/key-property")
					.HasAttribute("type", typeof(long).AssemblyQualifiedName);
		}

		[Test]
		public void KeyPropertyTypeIsSetToFullTypeNameIfTypeGeneric()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.CompositeId().KeyProperty(x => x.NullableLongId))
				.Element("class/composite-id/key-property")
					.HasAttribute("type", typeof(long?).AssemblyQualifiedName);
		}

		[Test]
		public void KeyManyToOneDefaults()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.CompositeId().KeyReference(x => x.Child))
				.Element("class/composite-id/key-many-to-one")
					.HasAttribute("name", "Child")
					.HasAttribute("class", typeof(CompIdChild).AssemblyQualifiedName);
		}

		[Test]
		public void KeyManyToOneExplicitColumnName()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.CompositeId().KeyReference(x => x.Child, "SomeColumn"))
				.Element("class/composite-id/key-many-to-one/column").HasAttribute("name", "SomeColumn");
		}

		[Test]
		public void MixedKeyPropertyAndManyToOne()
		{
			new MappingTester<CompIdTarget>()
				.ForMapping(c => c.CompositeId()
					.KeyProperty(x=>x.LongId)
					.KeyReference(x => x.Child))
				.Element("class/composite-id/key-property")
					.HasAttribute("name", "LongId")
				.RootElement.Element("class/composite-id/key-many-to-one")
					.HasAttribute("name", "Child");
		}

        [Test]
        public void IdIsAlwaysFirstElementInClass()
        {
            new MappingTester<CompIdTarget>()
                .ForMapping(m =>
                {
                    m.Map(x => x.DummyProp); // just a property in this case
                    m.CompositeId()
                        .KeyProperty(x => x.LongId)
                        .KeyReference(x => x.Child);
                })
                .Element("class/*[1]").HasName("composite-id");
        }



		public class CompIdTarget
		{
			public virtual long LongId { get; set; }
			public virtual long? NullableLongId { get; set; }
			public virtual CompIdChild Child { get; set; }
		    public virtual string DummyProp { get; set; }
		}

		public class CompIdChild
		{
			public virtual long ChildId { get; set; }
		}
	}
}
