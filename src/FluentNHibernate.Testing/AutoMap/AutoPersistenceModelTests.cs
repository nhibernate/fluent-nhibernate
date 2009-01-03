using System;
using FluentNHibernate.AutoMap;
using NUnit.Framework;
using SuperTypes = FluentNHibernate.AutoMap.TestFixtures.SuperTypes;
using FluentNHibernate.AutoMap.TestFixtures;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoPersistenceModelTests : BaseAutoPersistenceTests
    {
        [Test]
        public void TestAutoMapsIds()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleCustomColumn>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id").Exists();
        }

        [Test]
        public void TestAutoMapsProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//property").HasAttribute("name", "ExampleClassId");
        }

        [Test]
        public void TestAutoMapIgnoresProperties()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleCustomColumn>(c => c.IgnoreProperty(p => p.ExampleCustomColumnId));

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleCustomColumn>(autoMapper)
                .Element("//property").DoesntExist();
        }

        [Test]
        public void TestAutoMapManyToOne()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one").HasAttribute("name", "Parent");
        }

        [Test]
        public void TestAutoMapOneToMany()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleParentClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples");
        }

        [Test]
        public void TestAutoMapPropertyMergeOverridesId()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .ForTypesThatDeriveFrom<ExampleClass>(map => map.Id(c => c.Id, "Column"));

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id")
                .HasAttribute("column", "Column");
        }

        [Test]
        public void TestAutoMapPropertySetPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c=> c.GetPrimaryKeyName = p=> p.Name + "Id");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "Id")
                .HasAttribute("column", "IdId");
        }

        [Test]
        public void TestAutoMapPropertySetManyToOneKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c =>
                                    {
                                        c.GetForeignKeyName = p => p.Name + "Id";
                                    });

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//many-to-one")
                .HasAttribute("name", "Parent")
                .HasAttribute("column", "ParentId");
        }

        [Test]
        public void TestAutoMapPropertySetOneToManyKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c => c.GetForeignKeyNameOfParent =  t => t.Name + "Id");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleParentClass>(autoMapper)
                .Element("//bag")
                .HasAttribute("name", "Examples")
                .Element("//key")
                .HasAttribute("column", "ExampleParentClassId");
        }

        [Test]
        public void TestAutoMapPropertySetFindPrimaryKeyConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t == typeof(ExampleClass))
                .WithConvention(c => c.FindIdentity = p => p.Name == p.DeclaringType.Name + "Id" );

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/id")
                .HasAttribute("name", "ExampleClassId")
                .HasAttribute("column", "ExampleClassId");
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestInheritanceMappingSkipsSuperTypes()
        {
            var autoMapper = AutoPersistenceModel
                    .MapEntitiesFromAssemblyOf<ExampleClass>()
                    .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures.SuperTypes")
                    .WithConvention(c =>
                                        {
                                            c.IsBaseType = b => b == typeof(SuperTypes.SuperType);
                                        });

            autoMapper.Configure(cfg);

            new AutoMappingTester<SuperTypes.SuperType>(autoMapper);
        }

        [Test]
        public void TestInheritanceMapping()
        {
            var autoMapper = AutoPersistenceModel
                    .MapEntitiesFromAssemblyOf<ExampleClass>()
                    .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .HasAttribute("name", typeof(ExampleInheritedClass).AssemblyQualifiedName);

            tester.Element("class/joined-subclass/key")
                .HasAttribute("column", "ExampleClassId");
        }

        [Test]
        public void TestInheritanceMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                    .MapEntitiesFromAssemblyOf<ExampleClass>()
                    .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass/property")
                .HasAttribute("name", "ExampleProperty");
        }

        [Test]
        public void TestInheritanceMappingDoesntIncludeBaseTypeProperties()
        {
            var autoMapper = AutoPersistenceModel
                    .MapEntitiesFromAssemblyOf<ExampleClass>()
                    .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestInheritanceOverridingMappingProperties()
        {
            var autoMapper = AutoPersistenceModel
                    .MapEntitiesFromAssemblyOf<ExampleClass>()
                    .ForTypesThatDeriveFrom<ExampleClass>(t => t.JoinedSubClass<ExampleInheritedClass>("OverridenKey", p =>p.Map(c => c.ExampleProperty, "columnName")))
                    .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures");

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper).ToString();
            
            var tester = new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("class/joined-subclass")
                .ChildrenDontContainAttribute("name", "LineOne");
        }

        [Test]
        public void TestAutoMapClassSetCacheConvention()
        {
            var autoMapper = AutoPersistenceModel
                .MapEntitiesFromAssemblyOf<ExampleClass>()
                .Where(t => t.Namespace == "FluentNHibernate.AutoMap.TestFixtures")
                .WithConvention(c => c.DefaultCache = cache => cache.AsReadWrite());

            autoMapper.Configure(cfg);

            new AutoMappingTester<ExampleClass>(autoMapper)
                .Element("//cache").HasAttribute("usage", "read-write");
        }
    }
}