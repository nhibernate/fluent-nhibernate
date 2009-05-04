using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Utils
{
    [TestFixture]
    public class ReflectionHelperTests
    {
        [Test]
        public void CanGetProperty()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget>(x => x.Name);

            property.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetPropertyWithType()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget, string>(x => x.Name);

            property.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetPropertyWithConvert()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget>(x => (object)x.Name);

            property.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetPropertyWithTypeAndConvert()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget, object>(x => (object)x.Name);

            property.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetDynamicProperty()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget>(x => (string)x.ExtensionData["Name"]);

            property.ShouldBeOfType<DummyPropertyInfo>();
            property.Name.ShouldEqual("Name");
            property.PropertyType.ShouldEqual(typeof(string));
        }

        [Test]
        public void CanGetDynamicPropertyWithType()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget, string>(x => (string)x.ExtensionData["Name"]);

            property.ShouldBeOfType<DummyPropertyInfo>();
            property.Name.ShouldEqual("Name");
            property.PropertyType.ShouldEqual(typeof(string));
        }

        [Test]
        public void CanGetDynamicPropertyWithConvert()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget>(x => (int)x.ExtensionData["Name"]);

            property.ShouldBeOfType<DummyPropertyInfo>();
            property.Name.ShouldEqual("Name");
            property.PropertyType.ShouldEqual(typeof(int));
        }

        [Test]
        public void CanGetDynamicPropertyWithTypeAndConvert()
        {
            var property = ReflectionHelper.GetProperty<PropertyTarget, int>(x => (int)x.ExtensionData["Name"]);

            property.ShouldBeOfType<DummyPropertyInfo>();
            property.Name.ShouldEqual("Name");
            property.PropertyType.ShouldEqual(typeof(int));
        }
    }
}
