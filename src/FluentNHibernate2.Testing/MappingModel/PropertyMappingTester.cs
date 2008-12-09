using NUnit.Framework;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class PropertyMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var property = new PropertyMapping("Property1");
            property.ShouldBeValidAgainstSchema();
        }

        [Test]
        public void CanSpecifyLength()
        {
            var property = new PropertyMapping("Property1");
            property.Length = 50;

            property.Length.ShouldEqual(50);
            property.Hbm.length.ShouldEqual("50");
        }

        [Test]
        public void CanSpecifyNullability()
        {
            var property = new PropertyMapping("Property1");
            property.AllowNull = false;

            property.AllowNull.ShouldBeFalse();
            property.Hbm.notnull.ShouldBeTrue();

            property.AllowNull = true;
            property.AllowNull.ShouldBeTrue();
            property.Hbm.notnull.ShouldBeFalse();
        }


    }
}
