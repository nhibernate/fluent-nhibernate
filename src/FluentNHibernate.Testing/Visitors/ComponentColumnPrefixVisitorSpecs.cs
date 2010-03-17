using System.Linq;
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
        private ComponentColumnPrefixVisitor visitor;
        private ReferenceComponentMapping reference_with_a_prefix;
        private const string column_prefix = "{property}_";

        public override void establish_context()
        {
            visitor = new ComponentColumnPrefixVisitor();
            reference_with_a_prefix = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("PROPERTY", typeof(Target)).ToMember(), typeof(ComponentTarget), typeof(Target), column_prefix);

            var external_mapping = new ExternalComponentMapping(ComponentType.Component);
            external_mapping.AddProperty(property_with_column("propertyColumn"));

            reference_with_a_prefix.AssociateExternalMapping(external_mapping);
        }

        public override void because()
        {
            visitor.Visit(reference_with_a_prefix);
        }

        [Test]
        public void should_prefix_property_columns()
        {
            reference_with_a_prefix.Properties.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith("PROPERTY_"));
        }
    }

    [TestFixture]
    public class when_the_component_column_prefix_visitor_processes_a_reference_component_with_an_inner_reference_component_with_its_own_prefix : ComponentColumnPrefixVisitorSpec
    {
        private ComponentColumnPrefixVisitor visitor;
        private ReferenceComponentMapping reference_with_a_prefix;
        private const string first_prefix = "first_";
        private const string second_prefix = "second_";

        public override void establish_context()
        {
            visitor = new ComponentColumnPrefixVisitor();
            reference_with_a_prefix = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("PROPERTY", typeof(Target)).ToMember(), typeof(ComponentTarget), typeof(Target), first_prefix);
            reference_with_a_prefix.AssociateExternalMapping(new ExternalComponentMapping(ComponentType.Component));

            var sub_component = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("PROPERTY", typeof(Target)).ToMember(), typeof(ComponentTarget), typeof(Target), second_prefix);

            var sub_component_mapping = new ExternalComponentMapping(ComponentType.Component);
            sub_component_mapping.AddProperty(property_with_column("propertyColumn"));

            sub_component.AssociateExternalMapping(sub_component_mapping);

            reference_with_a_prefix.AssociateExternalMapping(new ExternalComponentMapping(ComponentType.Component));
            reference_with_a_prefix.AddComponent(sub_component);
        }

        public override void because()
        {
            visitor.Visit(reference_with_a_prefix);
        }

        [Test]
        public void should_prefix_sub_component_columns_with_both_prefixes()
        {
            reference_with_a_prefix.Components.SelectMany(x => x.Properties).SelectMany(x => x.Columns)
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
        private ComponentColumnPrefixVisitor visitor;
        private ReferenceComponentMapping reference_with_a_prefix;
        private const string column_prefix = "prefix_";

        public override void establish_context()
        {
            visitor = new ComponentColumnPrefixVisitor();
            reference_with_a_prefix = new ReferenceComponentMapping(ComponentType.Component, new DummyPropertyInfo("PROPERTY", typeof(Target)).ToMember(), typeof(ComponentTarget), typeof(Target), column_prefix);

            var external_mapping = new ExternalComponentMapping(ComponentType.Component);

            external_mapping.AddAny(any_with_column("anyColumn"));
            external_mapping.AddCollection(collection_with_column("collectionColumn"));

            var component = new ComponentMapping(ComponentType.Component);
            component.AddProperty(property_with_column("componentPropertyColumn"));

            external_mapping.AddComponent(component);
            external_mapping.AddProperty(property_with_column("propertyColumn"));
            external_mapping.AddReference(reference_with_column("referenceColumn"));

            reference_with_a_prefix.AssociateExternalMapping(external_mapping);
        }

        public override void because()
        {
            visitor.Visit(reference_with_a_prefix);
        }

        [Test]
        public void should_prefix_any_columns()
        {
            reference_with_a_prefix.Anys.SelectMany(x => x.IdentifierColumns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));

            reference_with_a_prefix.Anys.SelectMany(x => x.TypeColumns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }

        [Test]
        public void should_prefix_collection_columns()
        {
            reference_with_a_prefix.Collections.SelectMany(x => x.Key.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }

        [Test]
        public void should_prefix_columns_inside_an_inner_component()
        {
            reference_with_a_prefix.Components.SelectMany(x => x.Properties).SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }

        [Test]
        public void should_prefix_property_columns()
        {
            reference_with_a_prefix.Properties.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }

        [Test]
        public void should_prefix_reference_columns()
        {
            reference_with_a_prefix.References.SelectMany(x => x.Columns)
                .Each(x => x.Name.ShouldStartWith(column_prefix));
        }
    }

    public abstract class ComponentColumnPrefixVisitorSpec : Specification
    {
        protected AnyMapping any_with_column(string column)
        {
            var any = new AnyMapping();

            any.AddIdentifierDefaultColumn(new ColumnMapping { Name = column });
            any.AddTypeDefaultColumn(new ColumnMapping { Name = column });
            
            return any;
        }

        protected ICollectionMapping collection_with_column(string column)
        {
            var collection = new BagMapping();

            collection.Key = new KeyMapping();
            collection.Key.AddDefaultColumn(new ColumnMapping { Name = column });

            return collection;
        }

        protected PropertyMapping property_with_column(string column)
        {
            var property = new PropertyMapping();
            property.AddDefaultColumn(new ColumnMapping { Name = "propertyColumn" });
            return property;
        }

        protected ManyToOneMapping reference_with_column(string column)
        {
            var reference = new ManyToOneMapping();
            reference.AddDefaultColumn(new ColumnMapping { Name = "propertyColumn" });
            return reference;
        }

        protected class ComponentTarget { }
        protected class Target { }
    }
}