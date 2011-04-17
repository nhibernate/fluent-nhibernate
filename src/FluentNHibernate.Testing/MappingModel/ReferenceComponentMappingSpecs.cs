using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class when_a_reference_component_hasnt_been_associated_yet : Specification
    {
        public override void establish_context()
        {
            memberProperty = new DummyPropertyInfo("Component", typeof(Target)).ToMember();
            referenceComponentMapping = new ReferenceComponentMapping(ComponentType.Component, memberProperty, typeof(ComponentTarget), typeof(Target), null);
        }

        [Test]
        public void should_allow_access_to_the_type_property()
        {
            referenceComponentMapping.Type.ShouldEqual(typeof(ComponentTarget));
        }

        [Test]
        public void should_allow_access_to_the_name_property()
        {
            referenceComponentMapping.Name.ShouldEqual(memberProperty.Name);
        }

        [Test]
        public void should_allow_access_to_the_containing_entity_type_property()
        {
            referenceComponentMapping.ContainingEntityType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_allow_access_to_the_member_property()
        {
            referenceComponentMapping.Member.ShouldEqual(memberProperty);
        }

        private Member memberProperty;
        private ReferenceComponentMapping referenceComponentMapping;
        private class Target {}
        private class ComponentTarget {}
    }

    [TestFixture]
    public class when_a_reference_component_is_associated_to_a_external_component : Specification
    {
        public override void establish_context()
        {
            parentMapping = new ParentMapping();
            externalComponentMapping = new ExternalComponentMapping(ComponentType.Component);
            externalComponentMapping.Set(x => x.Access, Layer.Defaults, "access");
            externalComponentMapping.Set(x => x.Insert, Layer.Defaults, true);
            externalComponentMapping.Set(x => x.Lazy, Layer.Defaults, true);
            externalComponentMapping.Set(x => x.OptimisticLock, Layer.Defaults, true);
            externalComponentMapping.Set(x => x.Parent, Layer.Defaults, parentMapping);
            externalComponentMapping.Set(x => x.Unique, Layer.Defaults, true);
            externalComponentMapping.Set(x => x.Update, Layer.Defaults, true);
            externalComponentMapping.AddAny(new AnyMapping());
            externalComponentMapping.AddCollection(CollectionMapping.Bag());
            externalComponentMapping.AddComponent(new ComponentMapping(ComponentType.Component));
            externalComponentMapping.AddFilter(new FilterMapping());
            externalComponentMapping.AddOneToOne(new OneToOneMapping());
            externalComponentMapping.AddProperty(new PropertyMapping());
            externalComponentMapping.AddReference(new ManyToOneMapping());

            memberProperty = new DummyPropertyInfo("Component", typeof(Target)).ToMember();
            referenceComponentMapping = new ReferenceComponentMapping(ComponentType.Component, memberProperty, typeof(ComponentTarget), typeof(Target), "column-prefix");
        }

        public override void because()
        {
            referenceComponentMapping.AssociateExternalMapping(externalComponentMapping);
        }

        [Test]
        public void should_retain_type_information_from_before_the_association_occurred()
        {
            referenceComponentMapping.Type.ShouldEqual(typeof(ComponentTarget));
            referenceComponentMapping.Class.ShouldEqual(new TypeReference(typeof(ComponentTarget)));
            referenceComponentMapping.ContainingEntityType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_retain_member_information_from_before_the_association_occurred()
        {
            referenceComponentMapping.Member.ShouldEqual(memberProperty);
            referenceComponentMapping.Name.ShouldEqual(memberProperty.Name);
        }

        [Test]
        public void should_retail_column_prefix_information_from_before_the_association()
        {
            referenceComponentMapping.ColumnPrefix.ShouldEqual("column-prefix");
            referenceComponentMapping.HasColumnPrefix.ShouldBeTrue();
        }

        [Test]
        public void should_copy_all_the_properties_from_the_external_component()
        {
            referenceComponentMapping.Access.ShouldEqual("access");
            referenceComponentMapping.Insert.ShouldBeTrue();
            referenceComponentMapping.Lazy.ShouldBeTrue();
            referenceComponentMapping.OptimisticLock.ShouldBeTrue();
            referenceComponentMapping.Parent.ShouldEqual(parentMapping);
            referenceComponentMapping.Unique.ShouldBeTrue();
            referenceComponentMapping.Update.ShouldBeTrue();
        }

        [Test]
        public void should_copy_all_the_collections_from_the_external_component()
        {
            referenceComponentMapping.Anys.ItemsShouldBeEqual(externalComponentMapping.Anys);
            referenceComponentMapping.Collections.ItemsShouldBeEqual(externalComponentMapping.Collections);
            referenceComponentMapping.Components.ItemsShouldBeEqual(externalComponentMapping.Components);
            referenceComponentMapping.OneToOnes.ItemsShouldBeEqual(externalComponentMapping.OneToOnes);
            referenceComponentMapping.References.ItemsShouldBeEqual(externalComponentMapping.References);
        }

        private ReferenceComponentMapping referenceComponentMapping;
        private ExternalComponentMapping externalComponentMapping;
        private Member memberProperty;
        private ParentMapping parentMapping;

        private class ComponentTarget { }
        private class Target
        {
            public ComponentTarget Component { get; set; }
        }
    }
}