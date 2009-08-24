using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public class AutoMappedPropertyEnumerablesTests
    {
        [Test]
        public void ShouldSetNullableEnumsToNullable()
        {
            var automapper = AutoMap.Source(new StubTypeSource(typeof(Target)));

            automapper.CompileMappings();

            var mapping = automapper.BuildMappings().SelectMany(x => x.Classes).First();
            var property = mapping.Properties.First(x => x.Name == "NullableEnum");

            property.Columns.First().NotNull.ShouldBeFalse();
        }

        [Test]
        public void ShouldSetNullableEnumsToUseGenericEnumMapper()
        {
            var automapper = AutoMap.Source(new StubTypeSource(typeof(Target)));

            automapper.CompileMappings();

            var mapping = automapper.BuildMappings().SelectMany(x => x.Classes).First();
            var property = mapping.Properties.First(x => x.Name == "NullableEnum");

            property.Type.GetUnderlyingSystemType().ShouldEqual(typeof(GenericEnumMapper<Enum>));
        }

        [Test]
        public void ShouldSetEnumsToUseGenericEnumMapper()
        {
            var automapper = AutoMap.Source(new StubTypeSource(typeof(Target)));

            automapper.CompileMappings();

            var mapping = automapper.BuildMappings().SelectMany(x => x.Classes).First();
            var property = mapping.Properties.First(x => x.Name == "Enum");

            property.Type.GetUnderlyingSystemType().ShouldEqual(typeof(GenericEnumMapper<Enum>));
        }
    }

    internal enum Enum
    {

    }

    internal class Target
    {
        public Enum? NullableEnum { get; set; }
        public Enum Enum { get; set; }
    }
}