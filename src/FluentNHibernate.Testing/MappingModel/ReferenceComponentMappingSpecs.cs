using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class when_a_reference_component_hasnt_been_associated_yet : Specification
    {
        public override void establish_context()
        {
            member_property = new DummyPropertyInfo("Component", typeof(Target)).ToMember();
            reference_component_mapping = new ReferenceComponentMapping(ComponentType.Component, member_property, typeof(ComponentTarget), typeof(Target), null);
        }

        [Test]
        public void should_allow_access_to_the_type_property()
        {
            reference_component_mapping.Type.ShouldEqual(typeof(ComponentTarget));
        }

        [Test]
        public void should_allow_access_to_the_name_property()
        {
            reference_component_mapping.Name.ShouldEqual(member_property.Name);
        }

        [Test]
        public void should_allow_access_to_the_containing_entity_type_property()
        {
            reference_component_mapping.ContainingEntityType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_allow_access_to_the_member_property()
        {
            reference_component_mapping.Member.ShouldEqual(member_property);
        }

        private Member member_property;
        private ReferenceComponentMapping reference_component_mapping;
        private class Target {}
        private class ComponentTarget {}
    }

    [TestFixture]
    public class when_a_reference_component_is_associated_to_a_external_component : Specification
    {
        public override void establish_context()
        {
            parent_mapping = new ParentMapping();
            external_component_mapping = new ExternalComponentMapping(ComponentType.Component)
            {
                Access = "access",
                Insert = true,
                Lazy = true,
                OptimisticLock = true,
                Parent = parent_mapping,
                Unique = true,
                Update = true
            };
            external_component_mapping.AddAny(new AnyMapping());
            external_component_mapping.AddCollection(new BagMapping());
            external_component_mapping.AddComponent(new ComponentMapping(ComponentType.Component));
            external_component_mapping.AddFilter(new FilterMapping());
            external_component_mapping.AddOneToOne(new OneToOneMapping());
            external_component_mapping.AddProperty(new PropertyMapping());
            external_component_mapping.AddReference(new ManyToOneMapping());

            member_property = new DummyPropertyInfo("Component", typeof(Target)).ToMember();
            reference_component_mapping = new ReferenceComponentMapping(ComponentType.Component, member_property, typeof(ComponentTarget), typeof(Target), "column-prefix");
        }

        public override void because()
        {
            reference_component_mapping.AssociateExternalMapping(external_component_mapping);
        }

        [Test]
        public void should_retain_type_information_from_before_the_association_occurred()
        {
            reference_component_mapping.Type.ShouldEqual(typeof(ComponentTarget));
            reference_component_mapping.Class.ShouldEqual(new TypeReference(typeof(ComponentTarget)));
            reference_component_mapping.ContainingEntityType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_retain_member_information_from_before_the_association_occurred()
        {
            reference_component_mapping.Member.ShouldEqual(member_property);
            reference_component_mapping.Name.ShouldEqual(member_property.Name);
        }

        [Test]
        public void should_retail_column_prefix_information_from_before_the_association()
        {
            reference_component_mapping.ColumnPrefix.ShouldEqual("column-prefix");
            reference_component_mapping.HasColumnPrefix.ShouldBeTrue();
        }

        [Test]
        public void should_copy_all_the_properties_from_the_external_component()
        {
            reference_component_mapping.Access.ShouldEqual("access");
            reference_component_mapping.Insert.ShouldBeTrue();
            reference_component_mapping.Lazy.ShouldBeTrue();
            reference_component_mapping.OptimisticLock.ShouldBeTrue();
            reference_component_mapping.Parent.ShouldEqual(parent_mapping);
            reference_component_mapping.Unique.ShouldBeTrue();
            reference_component_mapping.Update.ShouldBeTrue();
        }

        [Test]
        public void should_copy_all_the_collections_from_the_external_component()
        {
            reference_component_mapping.Anys.ItemsShouldBeEqual(external_component_mapping.Anys);
            reference_component_mapping.Collections.ItemsShouldBeEqual(external_component_mapping.Collections);
            reference_component_mapping.Components.ItemsShouldBeEqual(external_component_mapping.Components);
            reference_component_mapping.OneToOnes.ItemsShouldBeEqual(external_component_mapping.OneToOnes);
            reference_component_mapping.References.ItemsShouldBeEqual(external_component_mapping.References);
        }

        private ReferenceComponentMapping reference_component_mapping;
        private ExternalComponentMapping external_component_mapping;
        private Member member_property;
        private ParentMapping parent_mapping;

        private class ComponentTarget { }
        private class Target
        {
            public ComponentTarget Component { get; set; }
        }
    }
}