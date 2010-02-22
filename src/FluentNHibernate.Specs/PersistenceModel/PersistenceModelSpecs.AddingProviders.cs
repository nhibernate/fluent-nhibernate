using FluentNHibernate.Mapping;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.PersistenceModel
{
    public class when_the_persistence_model_has_a_component_added_by_type : PersistenceModelSpec
    {
        Because of = () =>
            persistence_model.Add(typeof(MyComponentMap));

        It should_contain_the_mapping = () =>
            persistence_model.ContainsMapping(typeof(MyComponentMap)).ShouldBeTrue();
    }

    public class when_the_persistence_model_has_a_component_instance_added : PersistenceModelSpec
    {
        Because of = () =>
            persistence_model.Add(new MyComponentMap());

        It should_contain_the_mapping = () =>
            persistence_model.ContainsMapping(typeof(MyComponentMap)).ShouldBeTrue();
    }

    public class when_the_persistence_model_scans_a_source_for_types : PersistenceModelSpec
    {
        Because of = () =>
            persistence_model.AddMappingsFromSource(new StubTypeSource(new[]
            {
                typeof(MyComponentMap),
                typeof(MyClassMap),
                typeof(MySubclassMap),
                typeof(MyFilterMap),
            }));

        It should_contain_the_class_mapping = () =>
            persistence_model.ContainsMapping(typeof(MyClassMap)).ShouldBeTrue();

        It should_contain_the_component_mapping = () =>
            persistence_model.ContainsMapping(typeof(MyComponentMap)).ShouldBeTrue();

        It should_contain_the_subclass_mapping = () =>
            persistence_model.ContainsMapping(typeof(MySubclassMap)).ShouldBeTrue();

        It should_contain_the_filter_mapping = () =>
            persistence_model.ContainsMapping(typeof(MyFilterMap)).ShouldBeTrue();
    }

    public abstract class PersistenceModelSpec
    {
        Establish context = () =>
            persistence_model = new FluentNHibernate.PersistenceModel();

        protected static FluentNHibernate.PersistenceModel persistence_model;

        protected class MyComponentMap : ComponentMap<Target> { }
        protected class MyClassMap : ClassMap<Target> {}
        protected class MySubclassMap : SubclassMap<Target> {}
        protected class MyFilterMap : FilterDefinition {}
        protected class Target { }
    }
}