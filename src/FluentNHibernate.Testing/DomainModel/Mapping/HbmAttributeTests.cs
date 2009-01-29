using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class HbmAttributeTests
    {
        [Test]
        public void Can_specify_schema()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.SchemaIs("schema-name"))
                .RootElement.HasAttribute("schema", "schema-name");
        }

        [Test]
        public void Can_specify_default_cascade()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.DefaultAccess.AsCamelCaseField())
                .RootElement.HasAttribute("default-access", "field.camelcase");
        }

        [Test]
        public void Can_specify_auto_import_as_true()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.AutoImport())
                .RootElement.HasAttribute("auto-import", "true");
        }

        [Test]
        public void Can_specify_auto_import_as_false()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.AutoImport())
                .RootElement.HasAttribute("auto-import", "false");
        }

        [Test]
        public void Can_override_assembly_with_instance()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OverrideAssembly(typeof(string).Assembly))
                .RootElement.HasAttribute("assembly", "mscorlib");
        }

        [Test]
        public void Can_override_assembly_with_name()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OverrideAssembly("myAssembly"))
                .RootElement.HasAttribute("assembly", "myAssembly");
        }

        [Test]
        public void Can_override_namespace()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.OverrideNamespace("Some.Namespace"))
                .RootElement.HasAttribute("namespace", "Some.Namespace");
        }
    }
}