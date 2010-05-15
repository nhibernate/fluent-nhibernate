using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Steps
{
    [TestFixture]
    public class VersionStepTests
    {
        private VersionStep mapper;

        [SetUp]
        public void CreateMapper()
        {
            mapper = new VersionStep(new DefaultAutomappingConfiguration());
        }

        [Test]
        public void ShouldMapByteArray()
        {
            mapper.ShouldMap(typeof(Target).GetProperty("Version").ToMember()).ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayAsBinaryBlob()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version").ToMember());

            mapping.Version.Type.ShouldEqual(new TypeReference("BinaryBlob"));
        }

        [Test]
        public void ShouldMapByteArrayAsTimestampSqlType()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version").ToMember());

            SpecificationExtensions.ShouldBeTrue(mapping.Version.Columns.All(x => x.SqlType == "timestamp"));
        }

        [Test]
        public void ShouldMapByteArrayAsNotNull()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version").ToMember());

            SpecificationExtensions.ShouldBeTrue(mapping.Version.Columns.All(x => x.NotNull == true));
        }

        [Test]
        public void ShouldMapByteArrayWithUnsavedValueOfNull()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version").ToMember());

            mapping.Version.UnsavedValue.ShouldEqual(null);
        }

        [Test]
        public void ShouldMapInheritedByteArray()
        {
            var mapping = new ClassMapping { Type = typeof(SubTarget) };

            mapper.Map(mapping, typeof(SubTarget).GetProperty("Version").ToMember());

            Assert.That(mapping.Version, Is.Not.Null);
        }

        [Test]
        public void ShouldSetContainingEntityType()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, typeof(Target).GetProperty("Version").ToMember());

            mapping.Version.ContainingEntityType.ShouldEqual(typeof(Target));
        }

        private class Target
        {
            public byte[] Version { get; set; }
        }

        private class SubTarget : Target
        {}
    }

    [TestFixture]
    public class When_mapping_a_byte_array_version_property_and_the_version_property_is_on_a_base_class
    {
        private VersionStep mapper;

        [SetUp]
        public void CreateMapper()
        {
            mapper = new VersionStep(new DefaultAutomappingConfiguration());
        }

        [Test]
        public void ShouldMapByteArray()
        {
            mapper.ShouldMap(ReflectionHelper.GetMember<BaseEntityClass>(x => x.Version)).ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayAsBinaryBlob()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, ReflectionHelper.GetMember<BaseEntityClass>(x => x.Version));

            mapping.Version.Type.ShouldEqual(new TypeReference("BinaryBlob"));
        }

        [Test]
        public void ShouldMapByteArrayAsTimestampSqlType()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, ReflectionHelper.GetMember<BaseEntityClass>(x => x.Version));

            mapping.Version.Columns.All(x => x.SqlType == "timestamp").ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayAsNotNull()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, ReflectionHelper.GetMember<BaseEntityClass>(x => x.Version));

            mapping.Version.Columns.All(x => x.NotNull == true).ShouldBeTrue();
        }

        [Test]
        public void ShouldMapByteArrayWithUnsavedValueOfNull()
        {
            var mapping = new ClassMapping { Type = typeof(Target) };

            mapper.Map(mapping, ReflectionHelper.GetMember<BaseEntityClass>(x => x.Version));

            mapping.Version.UnsavedValue.ShouldEqual(null);
        }

        private class Target : BaseEntityClass
        {
        }

        private class BaseEntityClass
        {
            public byte[] Version { get; set; }
        }
    }

}