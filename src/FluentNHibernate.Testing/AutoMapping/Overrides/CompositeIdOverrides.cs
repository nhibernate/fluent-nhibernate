using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Automapping;
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

            model.CompileMappings();
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

            model.CompileMappings();
            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            classMapping.References.ShouldNotContain(x => x.Name == "Child");
        }
    }

    internal class CompositeIdEntity
    {
        public int ObjectId { get; set; }
        public int SecondId { get; set; }
        public Child Child { get; set; }
    }
}