using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Visitors
{
    [TestFixture]
    public class when_the_component_column_prefix_visitor_processes_a_reference_component_with_a_prefix_using_the_property_alias : ComponentColumnPrefixVisitorSpec
    {
        PersistenceModel model;
        IEnumerable<HibernateMapping> mappings;
        ClassMapping target_mapping;
        private const string column_prefix = "{property}_";

        public override void establish_context()
        {
            model = new PersistenceModel();

            var comp_map = new ComponentMap<ComponentTarget>();
            comp_map.Map(x => x.Property);

            model.Add(comp_map);

            var map = new ClassMap<Target>();
            map.Id(x => x.Id);
            map.Component(x => x.Component)
                .ColumnPrefix(column_prefix);
            model.Add(map);
        }

        public override void because()
        {
            mappings = model.BuildMappings();
            target_mapping = mappings
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(Target));
        }

        [Test]
        public void should_prefix_property_columns()
        {
            target_mapping.Components.Single()
                .Properties.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith("Component_"));
        }
    }

    [TestFixture]
    public class when_the_component_column_prefix_visitor_processes_a_reference_component_with_an_inner_reference_component_with_its_own_prefix : ComponentColumnPrefixVisitorSpec
    {
        PersistenceModel model;
        IEnumerable<HibernateMapping> mappings;
        ClassMapping target_mapping;
        private const string first_prefix = "first_";
        private const string second_prefix = "second_";

        public override void establish_context()
        {
            model = new PersistenceModel();

            var comp_map = new ComponentMap<ComponentTarget>();
            comp_map.Map(x => x.Property);
            comp_map.Component(x => x.Component)
                .ColumnPrefix(second_prefix);

            model.Add(comp_map);

            var comp2_map = new ComponentMap<Child>();
            comp2_map.Map(x => x.Property);

            model.Add(comp2_map);

            var map = new ClassMap<Target>();
            map.Id(x => x.Id);
            map.Component(x => x.Component)
                .ColumnPrefix(first_prefix);
            model.Add(map);
        }

        public override void because()
        {
            mappings = model.BuildMappings();
            target_mapping = mappings
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(Target));
        }

        [Test]
        public void should_prefix_sub_component_columns_with_both_prefixes()
        {
            target_mapping
                .Components.Single()
                .Components.Single()
                .Properties.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith(first_prefix + second_prefix));
        }
    }

    [TestFixture]
    public class when_the_component_column_prefix_visitor_processes_a_reference_component_after_already_processed_another : ComponentColumnPrefixVisitorSpec
    {
        private const string column_prefix = "prefix_";
        private ComponentColumnPrefixVisitor visitor;
        private ReferenceComponentMapping reference_with_a_prefix;
        private ReferenceComponentMapping reference_without_a_prefix;

        public override void establish_context()
        {
            visitor = new ComponentColumnPrefixVisitor();
            reference_with_a_prefix = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("PROPERTY", typeof(Target)).ToMember(), typeof(ComponentTarget), typeof(Target), column_prefix);
            reference_with_a_prefix.AssociateExternalMapping(new ExternalComponentMapping(ComponentType.Component));

            reference_without_a_prefix = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("PROPERTY", typeof(Target)).ToMember(), typeof(ComponentTarget), typeof(Target), null);

            var external_mapping = new ExternalComponentMapping(ComponentType.Component);
            external_mapping.AddProperty(property_with_column("propertyColumn"));

            reference_without_a_prefix.AssociateExternalMapping(external_mapping);
        }

        public override void because()
        {
            visitor.Visit(reference_with_a_prefix);
            visitor.Visit(reference_without_a_prefix);
        }

        [Test]
        public void shouldnt_use_the_original_prefix()
        {
            reference_without_a_prefix.Properties.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldNotStartWith(column_prefix));
        }
    }

    [TestFixture]
    public class when_the_component_column_prefix_visitor_processes_a_reference_component_with_a_prefix : ComponentColumnPrefixVisitorSpec
    {
        PersistenceModel model;
        IEnumerable<HibernateMapping> mappings;
        ClassMapping target_mapping;
        private const string column_prefix = "prefix_";

        public override void establish_context()
        {
            model = new PersistenceModel();
            var comp_map = new ComponentMap<ComponentTarget>();
            comp_map.Map(x => x.Property);
            comp_map.HasMany(x => x.Children);
            comp_map.Component(x => x.Component, c =>
                c.Map(x => x.Property));
            model.Add(comp_map);

            var map = new ClassMap<Target>();
            map.Id(x => x.Id);
            map.Component(x => x.Component)
                .ColumnPrefix(column_prefix);

            model.Add(map);
        }

        public override void because()
        {
            mappings = model.BuildMappings();
            target_mapping = mappings
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(Target));
        }

        [Test]
        public void should_prefix_collection_columns()
        {
            target_mapping.Components.Single().Collections.ShouldHaveCount(1);
            target_mapping.Components.Single().Collections
                .SelectMany(x => x.Key.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }

        [Test]
        public void should_prefix_columns_inside_an_inner_component()
        {
            target_mapping.Components.ShouldHaveCount(1);
            target_mapping.Components.SelectMany(x => x.Components).ShouldHaveCount(1);
            target_mapping.Components
                .SelectMany(x => x.Components)
                .SelectMany(x => x.Properties)
                .SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }

        [Test]
        public void should_prefix_property_columns()
        {
            target_mapping.Components.Single().Properties.ShouldHaveCount(1);
            target_mapping.Components.Single()
                .Properties.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }
    }

    public abstract class ComponentColumnPrefixVisitorSpec : Specification
    {
        protected AnyMapping any_with_column(string column)
        {
            var any = new AnyMapping();

            any.AddIdentifierColumn(Layer.Defaults, new ColumnMapping(column));
            any.AddTypeColumn(Layer.Defaults, new ColumnMapping(column));
            
            return any;
        }

        protected CollectionMapping collection_with_column(string column)
        {
            var collection = CollectionMapping.Bag();

            collection.Set(x => x.Key, Layer.Defaults, new KeyMapping());
            collection.Key.AddColumn(Layer.Defaults, new ColumnMapping(column));

            return collection;
        }

        protected PropertyMapping property_with_column(string column)
        {
            var property = new PropertyMapping();
            property.AddColumn(Layer.Defaults, new ColumnMapping("propertyColumn"));
            return property;
        }

        protected ManyToOneMapping reference_with_column(string column)
        {
            var reference = new ManyToOneMapping();
            reference.AddColumn(Layer.Defaults, new ColumnMapping("propertyColumn"));
            return reference;
        }

        protected class ComponentTarget
        {
            public string Property { get; set; }
            public IList<Child> Children { get; set; }
            public Child Component { get; set; }
        }
        protected class Child
        {
            public string Property { get; set; }
        }
        protected class Target
        {
            public int Id { get; set; }
            public ComponentTarget Component { get; set; }
        }
    }
}