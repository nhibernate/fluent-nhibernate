using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;
namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class KeyManyToOnePartTests
    {

        private KeyManyToOneMapping mapping;
        private KeyManyToOnePart keyPart;

        [SetUp]
        public void SetUp()
        {
            this.mapping = new KeyManyToOneMapping();
            this.keyPart = new KeyManyToOnePart(mapping);
        }

        [Test]
        public void ShouldSetForeignKey()
        {
            keyPart.ForeignKey("fk1");
            mapping.ForeignKey.ShouldEqual("fk1");
        }

        [Test]
        public void ShouldSetAccessStrategy()
        {
            keyPart.Access.Field();
            mapping.Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldSetLazy()
        {
            keyPart.Lazy();
            mapping.Lazy.ShouldBeTrue();            
        }

        [Test]
        public void ShouldSetNotLazy()
        {
            keyPart.Not.Lazy();
            mapping.Lazy.ShouldBeFalse();
        }

        [Test]
        public void ShouldSetName()
        {
            keyPart.Name("keypart1");
            mapping.Name.ShouldEqual("keypart1");
        }

        [Test]
        public void ShouldSetNotFound()
        {
            keyPart.NotFound.Ignore();
            mapping.NotFound.ShouldEqual("ignore");
        }



    }
}