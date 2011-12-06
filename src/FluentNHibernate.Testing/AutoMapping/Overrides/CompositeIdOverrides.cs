using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Overrides
{
    [TestFixture]
    public class CompositeIdOverrides
    {
        [Test]
        public void ShouldntMapPropertiesUsedInCompositeId()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(CompositeIdEntity) }))
                .Override<CompositeIdEntity>(o =>
                    o.CompositeId()
                        .KeyProperty(x => x.ObjectId)
                        .KeyProperty(x => x.SecondId));

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.Properties.ShouldNotContain(x => x.Name == "ObjectId");
            classMapping.Properties.ShouldNotContain(x => x.Name == "SecondId");
        }

        [Test]
        public void ShouldntMapReferencesUsedInCompositeId()
        {
            var model = AutoMap.Source(new StubTypeSource(new[] { typeof(CompositeIdEntity) }))
                .Override<CompositeIdEntity>(o =>
                    o.CompositeId()
                        .KeyReference(x => x.Child));

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.References.ShouldNotContain(x => x.Name == "Child");
        }

		[Test]
    	public void ShouldMapEnumIdAsAString()
		{
			var model = AutoMap.Source(new StubTypeSource(new[] {typeof(CompositeIdEntityWithEnum)}))
				.Override<CompositeIdEntityWithEnum>(o =>
					o.CompositeId()
						.KeyProperty(x => x.FirstId)
						.KeyProperty(x => x.SecondId));


			VerifyMapping(model, idMap =>
			{
				var firstKey = idMap.Keys.First();

				//this part is dumb because i'm asserting a specific implementation. i don't have any other way
				//of getting to the key type though
				firstKey.ShouldBeOfType(typeof(KeyPropertyMapping));
				var keyProp = (KeyPropertyMapping)firstKey;
				keyProp.Type.GetUnderlyingSystemType().ShouldEqual(typeof(GenericEnumMapper<>).MakeGenericType(typeof(SomeEnum)));
			});
		}

		[Test]
		public void ShouldMapEnumIdAsOverridenType()
		{
			var model = AutoMap.Source(new StubTypeSource(new[] { typeof(CompositeIdEntityWithEnum) }))
				.Override<CompositeIdEntityWithEnum>(o =>
					o.CompositeId()
						.KeyProperty(x => x.FirstId).CustomType<SomeEnum>()
						.KeyProperty(x => x.SecondId).CustomType<SomeEnum>());


			VerifyMapping(model, idMap =>
			{
				var firstKey = idMap.Keys.First();
				firstKey.ShouldBeOfType(typeof(KeyPropertyMapping));
				var firstKeyProp = (KeyPropertyMapping)firstKey;
				firstKeyProp.Type.GetUnderlyingSystemType().ShouldEqual(typeof(SomeEnum));
			});
		}

		private void VerifyMapping(AutoPersistenceModel model, Action<CompositeIdMapping> verifier)
		{
			var idMapping = model.BuildMappings()
								.First()
								.Classes
								.First()
								.Id
								;

			idMapping.ShouldBeOfType(typeof(CompositeIdMapping));
			verifier((CompositeIdMapping)idMapping);
		}
    }

    internal class CompositeIdEntity
    {
        public int ObjectId { get; set; }
        public int SecondId { get; set; }
        public Child Child { get; set; }
    }

	internal class CompositeIdEntityWithEnum
	{
		public SomeEnum FirstId { get; set; }
		public SomeEnum SecondId { get; set; }
	}

	internal enum SomeEnum
	{
		PossiblityOne,
		PossibilityTwo
	}
}