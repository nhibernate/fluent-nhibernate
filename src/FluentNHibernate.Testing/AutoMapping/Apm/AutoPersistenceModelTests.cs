using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;
using FluentNHibernate.Automapping.TestFixtures;
using ExampleClass=FluentNHibernate.Automapping.TestFixtures.ExampleClass;

namespace FluentNHibernate.Testing.AutoMapping.Apm
{
    [TestFixture]
    public partial class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
        [Test]
        public void CanSearchForOpenGenericTypes()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            autoMapper.BuildMappings();
            autoMapper.FindMapping(typeof(SomeOpenGenericType<>));
        }

        [Test]
        public void IdIsMappedFromGenericBaseClass()
        {
            var autoMapper = AutoMap.AssemblyOf<ClassUsingGenericBase>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures")
                .IgnoreBase(typeof(EntityBase<>));

            new AutoMappingTester<ClassUsingGenericBase>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id");
        }

        [Test]
        public void ShouldEscapeTableNames()
        {
            var autoMapper = AutoMap.AssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.Automapping.TestFixtures");

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class").HasAttribute("table", "`ExampleClass`");
        }

        [Test]
        public void ShouldUseGuidCombGeneratorForGuidIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithGuidId)));

            new AutoMappingTester<ClassWithGuidId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "guid.comb");
        }

        [Test]
        public void ShouldUseIdentityGeneratorForIntIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithIntId)));

            new AutoMappingTester<ClassWithIntId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void ShouldUseIdentityGeneratorForLongIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithLongId)));

            new AutoMappingTester<ClassWithLongId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "identity");
        }

        [Test]
        public void ShouldUseAssignedGeneratorForStringIds()
        {
            var autoMapper = AutoMap.Source(new StubTypeSource(typeof(ClassWithStringId)));

            new AutoMappingTester<ClassWithStringId>(autoMapper)
                .Element("class/id/generator").HasAttribute("class", "assigned");
        }

        private class JoinedSubclassConvention : IJoinedSubclassConvention
        {
            public void Apply(IJoinedSubclassInstance instance)
            {
                instance.Table("test-table");
                instance.Key.Column("test");
            }
        }

        private class TestIdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column("test");
            }
        }

        private class TestIdGeneratorConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Assigned();
            }
        }

        private class TestClassConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table("test");
            }
        }

        private class TestM2OConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column("test");
            }
        }

        private class TestO2MConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Table("test");
            }
        }
    }

    public class IgnorerOverride : IAutoMappingOverride<ExampleClass>
    {
        public void Override(AutoMapping<ExampleClass> mapping)
        {
            mapping.IgnoreProperty(x => x.LineOne);
        }
    }

    public class SomeOpenGenericType<T>
    {}
}
