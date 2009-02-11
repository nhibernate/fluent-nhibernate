using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapDynamicInsertTester
    {
        [Test]
        public void CanOverrideDynamicInsert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.DynamicInsert())
                .Element("class").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void CanOverrideNoDynamicInsert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.DynamicInsert())
                .Element("class").HasAttribute("dynamic-insert", "false");
        }
    }
}