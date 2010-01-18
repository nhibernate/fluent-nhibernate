using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class when_the_persistence_model_has_a_component_added_by_type : PersistenceModelSpec
    {
        public override void because()
        {
            persistence_model.Add(typeof(MyComponentMap));
        }

        [Test]
        public void should_contain_the_mapping()
        {
            persistence_model.ContainsMapping(typeof(MyComponentMap)).ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_the_persistence_model_has_a_component_instance_added : PersistenceModelSpec
    {
        public override void because()
        {
            persistence_model.Add(new MyComponentMap());
        }

        [Test]
        public void should_contain_the_mapping()
        {
            persistence_model.ContainsMapping(typeof(MyComponentMap)).ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_the_persistence_model_scans_a_source_for_types : PersistenceModelSpec
    {
        public override void because()
        {
            persistence_model.AddMappingsFromSource(new StubTypeSource(new[]
            {
                typeof(MyComponentMap),
                typeof(MyClassMap),
                typeof(MySubclassMap),
                typeof(MyFilterMap),
            }));
        }

        [Test]
        public void should_contain_the_class_mapping()
        {
            persistence_model.ContainsMapping(typeof(MyClassMap)).ShouldBeTrue();
        }

        [Test]
        public void should_contain_the_component_mapping()
        {
            persistence_model.ContainsMapping(typeof(MyComponentMap)).ShouldBeTrue();
        }

        [Test]
        public void should_contain_the_subclass_mapping()
        {
            persistence_model.ContainsMapping(typeof(MySubclassMap)).ShouldBeTrue();
        }

        [Test]
        public void should_contain_the_filter_mapping()
        {
            persistence_model.ContainsMapping(typeof(MyFilterMap)).ShouldBeTrue();
        }
    }

    public abstract class PersistenceModelSpec : Specification
    {
        public override void establish_context()
        {
            persistence_model = new PersistenceModel();
        }

        protected PersistenceModel persistence_model;

        protected class MyComponentMap : ComponentMap<Target> { }
        protected class MyClassMap : ClassMap<Target> {}
        protected class MySubclassMap : SubclassMap<Target> {}
        protected class MyFilterMap : FilterDefinition {}
        protected class Target { }
    }
}
