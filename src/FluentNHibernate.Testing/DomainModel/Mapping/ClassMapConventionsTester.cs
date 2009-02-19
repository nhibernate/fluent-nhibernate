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
            var visitor = new MappingVisitor();
            visitor.Conventions.DefaultLazyLoad = false;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .HasAttribute("default-lazy", "false");
        }

        [Test]
        public void DefaultLazyLoad_convention_should_not_override_direct_setting_on_classmap()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DefaultLazyLoad = false;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => c.SetHibernateMappingAttribute(ClassMap<MappedObject>.DefaultLazyAttributeKey, "true"))
                .HasAttribute("default-lazy", "true");
        }

        [Test]
        public void DynamicUpdateShouldntSetAttributeIfUnset()
        {
            var visitor = new MappingVisitor();
            // deliberately not setting DynamicUpdate

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").DoesntHaveAttribute("dynamic-update");
        }

        [Test]
        public void DynamicUpdateShouldSetAttributeIfTrue()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DynamicUpdate = type => true;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void DynamicUpdateShouldSetAttributeIfFalse()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DynamicUpdate = type => false;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-update", "false");
        }

        [Test]
        public void DynamicUpdateShouldntOverrideDirectSetting()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DynamicUpdate = type => true;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => c.SetAttribute("dynamic-update", "false"))
                .Element("class").HasAttribute("dynamic-update", "false");
        }

        [Test]
        public void DynamicInsertShouldntSetAttributeIfUnset()
        {
            var visitor = new MappingVisitor();
            // deliberately not setting DynamicInsert

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").DoesntHaveAttribute("dynamic-insert");
        }

        [Test]
        public void DynamicInsertShouldSetAttributeIfTrue()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DynamicInsert = type => true;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void DynamicInsertShouldSetAttributeIfFalse()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DynamicInsert = type => false;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").HasAttribute("dynamic-insert", "false");
        }

        [Test]
        public void DynamicInsertShouldntOverrideDirectSetting()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.DynamicInsert = type => true;

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => c.SetAttribute("dynamic-insert", "false"))
                .Element("class").HasAttribute("dynamic-insert", "false");
        }

        [Test]
        public void OptimisticLockShouldntSetAttributeIfNotSupplied()
        {
            var visitor = new MappingVisitor();

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").DoesntHaveAttribute("optimistic-lock");
        }

        [Test]
        public void OptimisticLockShouldSetAttributeIfSupplied()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.OptimisticLock = (type, locking) => locking.All();

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => { })
                .Element("class").HasAttribute("optimistic-lock", "all");
        }

        [Test]
        public void OptimisticLockShouldntOverrideDirectSetting()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.OptimisticLock = (type, locking) => locking.All();

            new MappingTester<MappedObject>()
                .UsingVisitor(visitor)
                .ForMapping(c => c.OptimisticLock.Dirty())
                .Element("class").HasAttribute("optimistic-lock", "dirty");
        }

        [Test]
        public void EnumsDontGetTypeOverriddenByConventionsIfExplicitlySet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Color).CustomTypeIs("int"))
                .Element("class/property[@name='Color']").HasAttribute("type", "int");
        }
    }
}