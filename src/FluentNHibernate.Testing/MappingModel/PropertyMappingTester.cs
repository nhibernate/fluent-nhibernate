using NUnit.Framework;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class PropertyMappingTester
    {

        [Test]
        public void CanSpecifyLength()
        {
            var property = new PropertyMapping { Name = "Property1" };
            property.Length = 50;

            property.Length.ShouldEqual(50);
        }

        [Test]
        public void CanSpecifyNullability()
        {
            var property = new PropertyMapping { Name = "Property1" };
            property.IsNotNullable = true;
            property.IsNotNullable.ShouldBeTrue();
        }


    }
}
