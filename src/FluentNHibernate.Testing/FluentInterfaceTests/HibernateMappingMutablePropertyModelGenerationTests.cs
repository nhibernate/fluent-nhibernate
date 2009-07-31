using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class HibernateMappingMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AutoImportShouldSetModelAutoImportPropertyToTrue()
        {
            HibernateMapping()
                .Mapping(m => m.AutoImport())
                .ModelShouldMatch(x => x.AutoImport.ShouldBeTrue());
        }

        [Test]
        public void NotAutoImportShouldSetModelAutoImportPropertyToFalse()
        {
            HibernateMapping()
                .Mapping(m => m.Not.AutoImport())
                .ModelShouldMatch(x => x.AutoImport.ShouldBeFalse());
        }

        [Test]
        public void DefaultAccessShouldSetModelDefaultAccessPropertyToValue()
        {
            HibernateMapping()
                .Mapping(m => m.DefaultAccess.Field())
                .ModelShouldMatch(x => x.DefaultAccess.ShouldEqual("field"));
        }

        [Test]
        public void DefaultCascadeShouldSetModelDefaultCascadePropertyToValue()
        {
            HibernateMapping()
                .Mapping(m => m.DefaultCascade.All())
                .ModelShouldMatch(x => x.DefaultCascade.ShouldEqual("all"));
        }

        [Test]
        public void DefaultLazyShouldSetModelDefaultLazyPropertyToTrue()
        {
            HibernateMapping()
                .Mapping(m => m.DefaultLazy())
                .ModelShouldMatch(x => x.DefaultLazy.ShouldBeTrue());
        }

        [Test]
        public void NotDefaultLazyShouldSetModelDefaultLazyPropertyToFalse()
        {
            HibernateMapping()
                .Mapping(m => m.Not.DefaultLazy())
                .ModelShouldMatch(x => x.DefaultLazy.ShouldBeFalse());
        }

        [Test]
        public void SchemaIsLazyShouldSetModelSchemaPropertyToValue()
        {
            HibernateMapping()
                .Mapping(m => m.Schema("schema"))
                .ModelShouldMatch(x => x.Schema.ShouldEqual("schema"));
        }

        [Test]
        public void CatalogShouldSetModelCatalogPropertyToValue()
        {
            HibernateMapping()
                .Mapping(m => m.Catalog("catalog"))
                .ModelShouldMatch(x => x.Catalog.ShouldEqual("catalog"));
        }

        [Test]
        public void NamespaceShouldSetModelNamespacePropertyToValue()
        {
            HibernateMapping()
                .Mapping(m => m.Namespace("namespace"))
                .ModelShouldMatch(x => x.Namespace.ShouldEqual("namespace"));
        }

        [Test]
        public void AssemblyShouldSetModelAssemblyPropertyToValue()
        {
            HibernateMapping()
                .Mapping(m => m.Assembly("assembly"))
                .ModelShouldMatch(x => x.Assembly.ShouldEndWith("assembly"));
        }
    }
}