using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class HbmAttributeTests
    {
        [Test]
        public void Can_specify_default_cascade()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HibernateMapping.DefaultAccess.CamelCaseField())
                .RootElement.HasAttribute("default-access", "field.camelcase");
        }

        [Test]
        public void Can_specify_auto_import_as_true()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HibernateMapping.AutoImport())
                .RootElement.HasAttribute("auto-import", "true");
        }

        [Test]
        public void Can_specify_auto_import_as_false()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HibernateMapping.Not.AutoImport())
                .RootElement.HasAttribute("auto-import", "false");
        }
    }
}