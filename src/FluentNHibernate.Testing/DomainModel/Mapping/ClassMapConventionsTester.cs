using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapConventionsTester
    {
        [Test]
        public void DefaultLazyLoad_should_be_false_if_set_by_convention()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DefaultLazyLoad = false)
                .ForMapping(c => { })
                .HasAttribute("default-lazy", "false");
        }

        [Test]
        public void DefaultLazyLoad_convention_should_not_override_direct_setting_on_classmap()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DefaultLazyLoad = false)
                .ForMapping(c => c.SetHibernateMappingAttribute("default-lazy", "true"))
                .HasAttribute("default-lazy", "true");
        }

        [Test]
        public void DynamicUpdateShouldntSetAttributeIfUnset()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c => { })
                .Element("class").DoesntHaveAttribute("dynamic-update");
        }

        [Test]
        public void DynamicUpdateShouldSetAttributeIfTrue()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DynamicUpdate = type => true)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void DynamicUpdateShouldSetAttributeIfFalse()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DynamicUpdate = type => false)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-update", "false");
        }

        [Test]
        public void DynamicUpdateShouldntOverrideDirectSetting()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DynamicUpdate = type => true)
                .ForMapping(c => c.SetAttribute("dynamic-update", "false"))
                .Element("class").HasAttribute("dynamic-update", "false");
        }

        [Test]
        public void DynamicInsertShouldntSetAttributeIfUnset()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c => { })
                .Element("class").DoesntHaveAttribute("dynamic-insert");
        }

        [Test]
        public void DynamicInsertShouldSetAttributeIfTrue()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DynamicInsert = type => true)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void DynamicInsertShouldSetAttributeIfFalse()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DynamicInsert = type => false)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-insert", "false");
        }

        [Test]
        public void DynamicInsertShouldntOverrideDirectSetting()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.DynamicInsert = type => true)
                .ForMapping(c => c.SetAttribute("dynamic-insert", "false"))
                .Element("class").HasAttribute("dynamic-insert", "false");
        }

        [Test]
        public void OptimisticLockShouldntSetAttributeIfNotSupplied()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c => { })
                .Element("class").DoesntHaveAttribute("optimistic-lock");
        }

        [Test]
        public void OptimisticLockShouldSetAttributeIfSupplied()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.OptimisticLock = (type, locking) => locking.All())
                .ForMapping(c => { })
                .Element("class").HasAttribute("optimistic-lock", "all");
        }

        [Test]
        public void OptimisticLockShouldntOverrideDirectSetting()
        {
            new MappingTester<MappedObject>()
                .WithConventions(conventions =>
                    conventions.OptimisticLock = (type, locking) => locking.All())
                .ForMapping(c => c.OptimisticLock.Dirty())
                .Element("class").HasAttribute("optimistic-lock", "dirty");
        }

        [Test]
        public void EnumsDontGetTypeOverriddenByConventionsIfExplicitlySet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Color).CustomTypeIs(typeof(int)))
                .Element("class/property[@name='Color']").HasAttribute("type", typeof(int).AssemblyQualifiedName);
        }
    }
}