using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping.Apm
{
    [TestFixture]
    public class GenericBaseClassTests
    {
        [Test]
        public void ShouldHaveCorrectIds()
        {
            var automapper =
                AutoMap.Source(new StubTypeSource(new[] { typeof(Parent<>), typeof(IntChild), typeof(GuidChild) }))
                    .IgnoreBase(typeof(Parent<>));

            automapper.CompileMappings();
            var mappings = automapper.BuildMappings();

            var intChild = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(IntChild));

            ((IdMapping)intChild.Id).Generator.Class.ShouldEqual("identity");

            var guidChild = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(GuidChild));

            ((IdMapping)guidChild.Id).Generator.Class.ShouldEqual("guid.comb");
        }
    }

    public class Parent<T>
    {
        public T Id { get; set; }
    }

    public class IntChild : Parent<int>
    { }

    public class GuidChild : Parent<Guid>
    { }
}