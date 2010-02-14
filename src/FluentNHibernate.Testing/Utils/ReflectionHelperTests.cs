using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Utils
{
    [TestFixture]
    public class ReflectionHelperTests
    {
        [Test]
        public void CanGetProperty()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget>(x => x.Name);

            member.MemberInfo.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetPropertyWithType()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget, string>(x => x.Name);

            member.MemberInfo.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetPropertyWithConvert()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget>(x => (object)x.Name);

            member.MemberInfo.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetPropertyWithTypeAndConvert()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget, object>(x => (object)x.Name);

            member.MemberInfo.ShouldEqual(typeof(PropertyTarget).GetProperty("Name"));
        }

        [Test]
        public void CanGetDynamicProperty()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget>(x => (string)x.ExtensionData["Name"]);

            member.MemberInfo.ShouldBeOfType<DummyPropertyInfo>();
            member.Name.ShouldEqual("Name");
            member.PropertyType.ShouldEqual(typeof(string));
        }

        [Test]
        public void CanGetDynamicPropertyWithType()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget, string>(x => (string)x.ExtensionData["Name"]);

            member.MemberInfo.ShouldBeOfType<DummyPropertyInfo>();
            member.Name.ShouldEqual("Name");
            member.PropertyType.ShouldEqual(typeof(string));
        }

        [Test]
        public void CanGetDynamicPropertyWithConvert()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget>(x => (int)x.ExtensionData["Name"]);

            member.MemberInfo.ShouldBeOfType<DummyPropertyInfo>();
            member.Name.ShouldEqual("Name");
            member.PropertyType.ShouldEqual(typeof(int));
        }

        [Test]
        public void CanGetDynamicPropertyWithTypeAndConvert()
        {
            var member = ReflectionHelper.GetMember<PropertyTarget, int>(x => (int)x.ExtensionData["Name"]);

            member.MemberInfo.ShouldBeOfType<DummyPropertyInfo>();
            member.Name.ShouldEqual("Name");
            member.PropertyType.ShouldEqual(typeof(int));
        }
    }
}
