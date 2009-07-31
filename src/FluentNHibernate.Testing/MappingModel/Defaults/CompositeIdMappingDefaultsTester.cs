using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Defaults
{
    [TestFixture]
    public class CompositeIdMappingDefaultsTester
    {
        [Test]
        public void MappedShouldDefaultToFalse()
        {
            var mapping = new CompositeIdMapping();
            mapping.Mapped.ShouldBeFalse();
        }

        [Test]
        public void UnsavedValueShouldDefaultToUndefined()
        {
            var mapping = new CompositeIdMapping();
            mapping.UnsavedValue.ShouldEqual("undefined");
        }
    }
}
