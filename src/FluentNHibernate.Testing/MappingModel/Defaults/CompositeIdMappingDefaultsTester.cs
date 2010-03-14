using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Defaults
{
    [TestFixture]
    public class CompositeIdMappingDefaultsTester
    {
        [Test]
        public void MappedShouldDefaultToFalseOnDefaultConstructor()
        {
            var mapping = new CompositeIdMapping();
            mapping.Mapped.ShouldBeFalse();
        }

        [Test]
        public void MappedShouldDefaultToTrueIfNameAttributeIsSet()
        {
            var store = new AttributeStore<CompositeIdMapping>();
            store.Set(x => x.Name, "someName");

            var mapping = new CompositeIdMapping(store.CloneInner());
            mapping.Mapped.ShouldBeTrue();
        }

        [Test]
        public void MappedShouldDefaultToFalseIfNameAttributeIsBlank()
        {
            var store = new AttributeStore<CompositeIdMapping>();
            store.Set(x => x.Name, string.Empty);

            var mapping = new CompositeIdMapping(store.CloneInner());
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
