using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface
{
    public class when_creating_the_mapping_for_a_component_using_component_map
    {
        Establish context = () =>
        {
            component = new ComponentMap<Target>();
            component.Map(x => x.a_property);
            component.ReferencesAny(x => x.an_any).EntityIdentifierColumn("x").EntityTypeColumn("y").IdentityType(typeof(Target));
            component.HasMany(x => x.a_collection);
            component.Component(x => x.a_component);
            component.HasOne(x => x.a_one_to_one);
            component.References(x => x.a_reference);
        };

        Because of = () =>
            mapping = (component as IExternalComponentMappingProvider).GetComponentMapping();

        It should_create_an_external_component_mapping = () =>
            mapping.ShouldBeOfType<ExternalComponentMapping>();

        It should_add_properties_to_the_properties_collection = () =>
            mapping.Properties.ShouldContain(x => x.Name == "a_property");

        It should_add_anys_to_the_anys_collection = () =>
            mapping.Anys.ShouldContain(x => x.Name == "an_any");

        It should_add_collections_to_the_collections_collection = () =>
            mapping.Collections.ShouldContain(x => x.Name == "a_collection");

        It should_add_components_to_the_components_collection = () =>
            mapping.Components.ShouldContain(x => x.Name == "a_component");

        It should_add_one_to_ones_to_the_one_to_ones_collection = () =>
            mapping.OneToOnes.ShouldContain(x => x.Name == "a_one_to_one");

        It should_add_references_to_the_references_collection = () =>
            mapping.References.ShouldContain(x => x.Name == "a_reference");
        
        private static ComponentMap<Target> component;
        private static ComponentMapping mapping;

        private class Target
        {
            public string a_property { get; set; }
            public Target an_any { get; set; }
            public ComponentTarget a_component { get; set; }
            public Target a_one_to_one { get; set; }
            public Target a_reference { get; set; }
            public IEnumerable<Target> a_collection { get; set; }
        }

        class ComponentTarget
        {}
    }

    public class when_mapping_a_component_in_an_entity_without_defining_any_mappings_for_the_component
    {
        Establish context = () =>
        {
            classmap = new ClassMap<Target>();
            classmap.Id(x => x.Id);
            classmap.Component(x => x.Component);
        };

        Because of = () =>
            mapping = (classmap as IMappingProvider).GetClassMapping()
                .Components.First();

        It should_create_a_reference_component_mapping = () =>
            mapping.ShouldBeOfType<ReferenceComponentMapping>();

        It should_store_the_property_in_the_reference_component_mapping = () =>
            (mapping as ReferenceComponentMapping).Member.Name.ShouldBeEqualIgnoringCase("Component");

        private static ClassMap<Target> classmap;
        private static IComponentMapping mapping;

        private class Target
        {
            public int Id { get; set; }
            public Component Component { get; set;}
        }

        private class Component {}
    }

    public class when_compiling_the_mappings_with_a_reference_component_in_a_subclass
    {
        Establish context = () =>
        {
            var component_map = new ComponentMap<Component>();
            component_map.Map(x => x.Property);

            var class_map = new ClassMap<Target>();
            class_map.Id(x => x.Id);

            var subclass_map = new SubclassMap<TargetChild>();
            subclass_map.Component(x => x.Component);

            persistence_model = new FluentNHibernate.PersistenceModel();
            persistence_model.Add(class_map);
            persistence_model.Add(subclass_map);
            persistence_model.Add(component_map);
        };

        Because of = () =>
        {
            mappings = persistence_model.BuildMappings();
            class_mapping = mappings.SelectMany(x => x.Classes).First();
        };

        It should_add_the_subclass_to_the_class = () =>
            class_mapping.Subclasses.Count().ShouldEqual(1);

        It should_merge_the_delegated_component_mapping_with_the_unassociated_component_mapping_from_the_component_map = () =>
        {
            var component_mapping = class_mapping.Subclasses.Single().Components.First();

            component_mapping.Member.Name.ShouldBeEqualIgnoringCase("Component");
            component_mapping.Properties.ShouldContain(x => x.Name == "Property");
        };

        private static FluentNHibernate.PersistenceModel persistence_model;
        private static IEnumerable<HibernateMapping> mappings;
        private static ClassMapping class_mapping;

        private class Target
        {
            public int Id { get; set; }
        }

        private class TargetChild : Target
        {
            public Component Component { get; set; }
        }

        private class Component
        {
            public string Property { get; set; }
        }
    }

    public class when_compiling_the_mappings_with_a_nested_reference_component_in_a_component_map
    {
        Establish context = () =>
        {
            component_map = new ComponentMap<Component>();
        };

        Because of = () =>
            ex = Catch.Exception(() => component_map.Component(x => x.Compo));

        It should_throw_a_not_supported_exception = () =>
            ex.ShouldBeOfType<NotSupportedException>();

        static ComponentMap<Component> component_map;
        static Exception ex;

        private class Component
        {
            public Component Compo { get; set; }
        }
    }

    public class when_compiling_the_mappings_with_a_reference_component_and_a_related_external_component
    {
        Establish context = () =>
        {
            var component_map = new ComponentMap<Component>();
            component_map.Map(x => x.Property);

            var class_map = new ClassMap<Target>();
            class_map.Id(x => x.Id);
            class_map.Component(x => x.Component);

            persistence_model = new FluentNHibernate.PersistenceModel();
            persistence_model.Add(class_map);
            persistence_model.Add(component_map);
        };

        Because of = () =>
        {
            mappings = persistence_model.BuildMappings();
            class_mapping = mappings.SelectMany(x => x.Classes).First();
        };

        It should_merge_the_delegated_component_mapping_with_the_unassociated_component_mapping_from_the_component_map = () =>
        {
            var component_mapping = class_mapping.Components.First();

            component_mapping.Member.Name.ShouldBeEqualIgnoringCase("Component");
            component_mapping.Properties.ShouldContain(x => x.Name == "Property");
        };

        private static FluentNHibernate.PersistenceModel persistence_model;
        private static IEnumerable<HibernateMapping> mappings;
        private static ClassMapping class_mapping;

        private class Target
        {
            public int Id { get; set; }
            public Component Component { get; set; }
        }

        private class Component
        {
            public string Property { get; set; }
        }
    }

    public class when_compiling_the_mappings_with_two_of_the_same_reference_component_and_a_related_external_component
    {
        Establish context = () =>
        {
            var component_map = new ComponentMap<Component>();
            component_map.Map(x => x.Property, "PROP");

            var class_map = new ClassMap<Target>();
            class_map.Id(x => x.Id);
            class_map.Component(x => x.ComponentA)
                .ColumnPrefix("A_");
            class_map.Component(x => x.ComponentB)
                .ColumnPrefix("B_");

            persistence_model = new FluentNHibernate.PersistenceModel();
            persistence_model.Add(class_map);
            persistence_model.Add(component_map);
        };

        Because of = () =>
        {
            mappings = persistence_model.BuildMappings();
            class_mapping = mappings.SelectMany(x => x.Classes).First();
        };

        It should_merge_the_component_mappings_with_the_mapping_from_the_component_map = () =>
            class_mapping.Components.Select(x => x.Name).ShouldContain("ComponentA", "ComponentB");

        It should_use_the_column_prefixes_for_the_columns = () =>
        {
            class_mapping.Components.First(x => x.Name == "ComponentA")
                .Properties.SelectMany(x => x.Columns)
                .Select(x => x.Name)
                .ShouldContain("A_PROP");
            class_mapping.Components.First(x => x.Name == "ComponentB")
                .Properties.SelectMany(x => x.Columns)
                .Select(x => x.Name)
                .ShouldContain("B_PROP");
        };

        private static FluentNHibernate.PersistenceModel persistence_model;
        private static IEnumerable<HibernateMapping> mappings;
        private static ClassMapping class_mapping;

        private class Target
        {
            public int Id { get; set; }
            public Component ComponentA { get; set; }
            public Component ComponentB { get; set; }
        }

        private class Component
        {
            public string Property { get; set; }
        }
    }
}