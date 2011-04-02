using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class MemberAccessResolverTests
    {
        [Test]
        public void AutoProperty()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, string>(x => x.AutoProperty))
                .ShouldEqual(FluentNHibernate.Mapping.Access.Property);
        }

        [Test]
        public void AutoPropertyPrivateSetter()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, string>(x => x.AutoPropertyPrivateSetter))
                .ShouldEqual(FluentNHibernate.Mapping.Access.BackField);
        }

        [Test]
        public void Property()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, string>(x => x.Property))
                .ShouldEqual(FluentNHibernate.Mapping.Access.Property);
        }

        [Test]
        public void PropertyPrivateSetter()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, string>(x => x.PropertyPrivateSetter))
                .ShouldEqual(FluentNHibernate.Mapping.Access.ReadOnlyPropertyThroughCamelCaseField());
        }

        [Test]
        public void PropertyNoSetter()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, string>(x => x.PropertyNoSetter))
                .ShouldEqual(FluentNHibernate.Mapping.Access.ReadOnlyPropertyThroughCamelCaseField());
        }

        [Test]
        public void Field()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, string>(x => x.Field))
                .ShouldEqual(FluentNHibernate.Mapping.Access.Field);
        }

        [Test]
        public void Method()
        {
            MemberAccessResolver.Resolve(ReflectionExtensions.ToMember<Example, IEnumerable<Example>>(x => x.GetViaMethod()))
                .ShouldEqual(FluentNHibernate.Mapping.Access.Field);
        }

#pragma warning disable 649
        class Example
        {
            string property;
            string propertyPrivateSetter;
            string propertyNoSetter;

            public string Field;
            IEnumerable<Example> viaMethod;

            public string AutoProperty { get; set; }
            public string AutoPropertyPrivateSetter { get; private set; }

            public string Property
            {
                get { return property; }
                set { property = value; }
            }

            public string PropertyPrivateSetter
            {
                get { return propertyPrivateSetter; }
                private set { propertyPrivateSetter = value; }
            }

            public string PropertyNoSetter
            {
                get { return propertyNoSetter; }
            }

            public IEnumerable<Example> GetViaMethod()
            {
                return viaMethod;
            }
        }
#pragma warning restore 649
    }
}