using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class UnionSubclassTester
    {
        [SetUp]
        public void SetUp()
        {}


        [Test]
        public void MapsSubclassWithUnionSubClass()
        {
            new MappingTester<MappedObject>()
                .SubClassMapping<MappedObjectSubclass>(m =>
                {
                    m.Map(x => x.SubclassProperty);
                })
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.UseUnionSubclassForInheritanceMapping();


                }).Element("class/union-subclass").Exists();
        }

        [Test]
        public void ShouldAllowEntityNameToBeSetOnUnionSubclasses()
        {
            new MappingTester<MappedObject>()
                .SubClassMapping<MappedObjectSubclass>(m =>
                {
                    m.EntityName("name");
                })
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.UseUnionSubclassForInheritanceMapping();


                }).Element("class/union-subclass")
                    .HasAttribute("entity-name", "name");
        }
    }
}