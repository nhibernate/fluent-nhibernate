//using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Helpers;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapConventionsTester
    {
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
                .Conventions(conventions => conventions.Add(DynamicUpdate.AlwaysTrue()))
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void DynamicUpdateShouldSetAttributeIfFalse()
        {
            new MappingTester<MappedObject>()
                .Conventions(conventions => conventions.Add(DynamicUpdate.AlwaysFalse()))
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-update", "false");
        }

        [Test]
        public void DynamicUpdateShouldntOverrideDirectSetting()
        {
            new MappingTester<MappedObject>()
                .Conventions(conventions => conventions.Add(DynamicUpdate.AlwaysTrue()))
                .ForMapping(c => c.Not.DynamicUpdate())
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
                .Conventions(conventions => conventions.Add(DynamicInsert.AlwaysTrue()))
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void DynamicInsertShouldSetAttributeIfFalse()
        {
            new MappingTester<MappedObject>()
                .Conventions(conventions => conventions.Add(DynamicInsert.AlwaysFalse()))
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-insert", "false");
        }

        [Test]
        public void DynamicInsertShouldntOverrideDirectSetting()
        {
            new MappingTester<MappedObject>()
                .Conventions(conventions => conventions.Add(DynamicInsert.AlwaysTrue()))
                .ForMapping(c => c.Not.DynamicInsert())
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
                .Conventions(conventions => conventions.Add(OptimisticLock.Is(x => x.All())))
                .ForMapping(c => { })
                .Element("class").HasAttribute("optimistic-lock", "all");
        }

        [Test]
        public void OptimisticLockShouldntOverrideDirectSetting()
        {
            new MappingTester<MappedObject>()
                .Conventions(conventions => conventions.Add(OptimisticLock.Is(x => x.All())))
                .ForMapping(c => c.OptimisticLock.Dirty())
                .Element("class").HasAttribute("optimistic-lock", "dirty");
        }

        [Test]
        public void EnumsDontGetTypeOverriddenByConventionsIfExplicitlySet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Color).CustomType(typeof(int)))
                .Element("class/property[@name='Color']").HasAttribute("type", typeof(int).Name);
        }
    }
}