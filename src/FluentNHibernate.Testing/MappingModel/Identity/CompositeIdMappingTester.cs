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

            mapping.Name = "someName";
            mapping.Mapped.ShouldBeTrue();
        }

        [Test]
        public void SettingNameToBlankValueShouldSetMappedToFalse()
        {
            var mapping = new CompositeIdMapping();
            mapping.Mapped = true;

            mapping.Name = null;
            mapping.Mapped.ShouldBeFalse();
        }
    }
}