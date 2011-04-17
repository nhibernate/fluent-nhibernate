using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;
namespace FluentNHibernate.Testing.MappingModel.Identity
{
    [TestFixture]
    public class CompositeIdMappingTester
    {
        [Test]
        public void SettingNameShouldSetMappedToTrue()
        {
            var mapping = new CompositeIdMapping();

            mapping.Set(x => x.Name, Layer.Defaults, "someName");
            mapping.Mapped.ShouldBeTrue();
        }

        [Test]
        public void SettingNameToBlankValueShouldSetMappedToFalse()
        {
            var mapping = new CompositeIdMapping();
            mapping.Set(x => x.Name, Layer.Defaults, null);
            mapping.Mapped.ShouldBeFalse();
        }
    }
}