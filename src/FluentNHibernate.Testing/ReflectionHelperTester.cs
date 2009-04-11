using System.Reflection;
using FluentNHibernate.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing
{
    [TestFixture]
    public class ReflectionHelperTester
    {
        private class TestClass
        {
            public string PublicField = string.Empty;
            public string PublicProperty { get; set; }
        }

        [Test]
        public void Can_get_field()
        {
            FieldInfo fieldInfo = ReflectionHelper.GetField<TestClass>(x => x.PublicField);
            fieldInfo.ShouldNotBeNull();
        }

        [Test]
        public void Can_get_member()
        {
            MemberInfo member;
            member = ReflectionHelper.GetMember<TestClass>(x => x.PublicField);
            member = ReflectionHelper.GetMember<TestClass>(x => x.PublicProperty);
        }
    }
}