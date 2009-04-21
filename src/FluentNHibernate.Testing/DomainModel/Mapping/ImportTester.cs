using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ImportTester
    {
        [Test]
        public void ShouldAddImportElementsBeforeClass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.ImportType<SecondMappedObject>())
                .Element("import")
                .Exists()
                .HasAttribute("class", typeof(SecondMappedObject).AssemblyQualifiedName);
        }

        [Test]
        public void ShouldntAddImportElementsInsideClass()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.ImportType<SecondMappedObject>())
                .Element("class/import").DoesntExist();
        }

        [Test]
        public void ShouldAddRenameAttributeWhenDifferentNameSpecified()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => x.ImportType<SecondMappedObject>().As("MappedObject"))
                .Element("import").HasAttribute("rename", "MappedObject");
        }
    }
}