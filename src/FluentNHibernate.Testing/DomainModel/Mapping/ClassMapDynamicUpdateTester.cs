using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapDynamicUpdateTester
    {
        [Test]
        public void CanOverrideDynamicUpdate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.DynamicUpdate())
                .Element("class").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void CanOverrideNoDynamicUpdate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.DynamicUpdate())
                .Element("class").HasAttribute("dynamic-update", "false");
        }
    }
}