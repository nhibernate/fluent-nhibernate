using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class AutoMapVersionTester
    {
        private AutoMapVersion mapper;

        [SetUp]
        public void CreateMapper()
        {
            mapper = new AutoMapVersion();
        }

        [Test]
        public void ShouldMapByteArray()
        {
            mapper.MapsProperty(typeof(Target).GetProperty("Version")).ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayAsBinaryBlob()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version"));

            mapping.Version.Type.ShouldEqual(new TypeReference("BinaryBlob"));
        }

        [Test]
        public void ShouldMapByteArrayAsTimestampSqlType()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version"));

            mapping.Version.Columns.All(x => x.SqlType == "timestamp").ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayAsNotNull()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version"));

            mapping.Version.Columns.All(x => x.NotNull == true).ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayWithUnsavedValueOfNull()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version"));

            mapping.Version.UnsavedValue.ShouldEqual(null);
        }

        private class Target
        {
            public byte[] Version { get; set; }
        }
    }
}