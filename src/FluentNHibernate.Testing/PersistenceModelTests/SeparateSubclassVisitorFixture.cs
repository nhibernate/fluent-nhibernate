using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class SeparateSubclassVisitorFixture
    {
        private IList<IIndeterminateSubclassMappingProvider> providers;
        private ClassMapping fooMapping;

        [SetUp]
        public void SetUp()
        {
            providers = new List<IIndeterminateSubclassMappingProvider>();
            fooMapping = ((IMappingProvider)new FooMap()).GetClassMapping();
        }

        [Test, Ignore("Issue 340: http://code.google.com/p/fluent-nhibernate/issues/detail?id=340")]
        public void Should_add_subclass_that_implement_the_parent_interface()
        {
            providers.Add(new StringFooMap());
            var sut = new SeparateSubclassVisitor(providers);
            sut.ProcessClass(fooMapping);
            Assert.AreEqual(1, fooMapping.Subclasses.Count());
            Assert.AreEqual(1, fooMapping.Subclasses.Where(sub => sub.Type.Equals(typeof(Foo<string>))).Count());
        }

        [Test]
        public void Should_not_add_subclassmap_that_does_not_implement_parent_interface()
        {
            providers.Add(new OtherMap());
            var sut = new SeparateSubclassVisitor(providers);
            sut.ProcessClass(fooMapping);
            Assert.AreEqual(0, fooMapping.Subclasses.Count());
        }

        private interface IFoo
        {
        }

        private interface IOther
        {
            IFoo Foo { get; }
        }

        private class Base
        {
        }

        private class Other : Base { }

        private class Foo<T> : Base, IFoo
        {
        }

        private class FooMap : ClassMap<IFoo>
        {
        }

        private abstract class SubFooMap<T> : SubclassMap<Foo<T>>
        {
        }

        private class StringFooMap : SubFooMap<string>
        {
        }

        private class OtherMap : SubclassMap<Other>
        {
        }
    }
}
