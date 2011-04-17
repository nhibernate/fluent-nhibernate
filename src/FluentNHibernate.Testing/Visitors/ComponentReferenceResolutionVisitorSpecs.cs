using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.Utils;
using FluentNHibernate.Visitors;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Visitors
{
    [TestFixture]
    public class when_the_component_reference_resolution_visitor_processes_a_component_reference_with_a_corresponding_external_component : ComponentReferenceResolutionVisitorSpec
    {
        public override void establish_context()
        {
            externalComponentMapping = new ExternalComponentMapping(ComponentType.Component);
            externalComponentMapping.Set(x => x.Type, Layer.Defaults, typeof(ComponentTarget));
            var externalComponent = Stub<IExternalComponentMappingProvider>.Create(cfg =>
                cfg.Stub(x => x.GetComponentMapping()).Return(externalComponentMapping));

            visitor = new ComponentReferenceResolutionVisitor(new [] { new ComponentMapComponentReferenceResolver() }, new[] { externalComponent });
            referenceComponentMapping = new ReferenceComponentMapping(ComponentType.Component, null, null, null, null);
        }

        public override void because()
        {
            visitor.ProcessComponent(referenceComponentMapping);
        }

        [Test]
        public void should_associate_external_mapping_with_reference_mapping()
        {
            referenceComponentMapping.IsAssociated.ShouldBeTrue();
        }

        private ReferenceComponentMapping referenceComponentMapping;
        private ExternalComponentMapping externalComponentMapping;
    }

    [TestFixture]
    public class when_the_component_reference_resolution_visitor_processes_a_component_reference_without_a_corresponding_external_component : ComponentReferenceResolutionVisitorSpec
    {
        public override void establish_context()
        {
            visitor = new ComponentReferenceResolutionVisitor(new[] { new ComponentMapComponentReferenceResolver() }, new IExternalComponentMappingProvider[0]);
            memberProperty = new DummyPropertyInfo("Component", typeof(ComponentTarget)).ToMember();
        }

        public override void because()
        {
            visitor.ProcessComponent(new ReferenceComponentMapping(ComponentType.Component, memberProperty, typeof(ComponentTarget), typeof(Target), null));
        }

        [Test]
        public void should_throw_a_missing_external_component_exception()
        {
            thrown_exception.ShouldNotBeNull();
            thrown_exception.ShouldBeOfType<MissingExternalComponentException>();
        }

        [Test]
        public void should_throw_exception_with_correct_message()
        {
            thrown_exception.Message.ShouldEqual("Unable to find external component for 'ComponentTarget', referenced from property 'Component' of 'Target'.");
        }

        [Test]
        public void should_have_type_for_missing_component_in_exception()
        {
            ((MissingExternalComponentException)thrown_exception).ReferencedComponentType.ShouldEqual(typeof(ComponentTarget));
        }

        [Test]
        public void should_have_type_for_the_reference_in_exception()
        {
            ((MissingExternalComponentException)thrown_exception).SourceType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_have_property_from_the_reference_in_exception()
        {
            ((MissingExternalComponentException)thrown_exception).SourceMember.ShouldEqual(memberProperty);
        }
    }

    [TestFixture]
    public class when_the_component_reference_resolution_visitor_processes_a_component_reference_with_multiple_corresponding_external_components : ComponentReferenceResolutionVisitorSpec
    {
        public override void establish_context()
        {
            var externalComponentOne = Stub<IExternalComponentMappingProvider>.Create(cfg =>
            {
                var externalComponentMapping = new ExternalComponentMapping(ComponentType.Component);
                externalComponentMapping.Set(x => x.Type, Layer.Defaults, typeof(ComponentTarget));
                cfg.Stub(x => x.GetComponentMapping()).Return(externalComponentMapping);
                cfg.Stub(x => x.Type).Return(typeof(ComponentTarget));
            });
            var externalComponentTwo = Stub<IExternalComponentMappingProvider>.Create(cfg =>
            {
                var externalComponentMapping = new ExternalComponentMapping(ComponentType.Component);
                externalComponentMapping.Set(x => x.Type, Layer.Defaults, typeof(ComponentTarget));
                cfg.Stub(x => x.GetComponentMapping()).Return(externalComponentMapping);
                cfg.Stub(x => x.Type).Return(typeof(ComponentTarget));
            });

            visitor = new ComponentReferenceResolutionVisitor(new[] { new ComponentMapComponentReferenceResolver() }, new[] { externalComponentOne, externalComponentTwo });
            memberProperty = new DummyPropertyInfo("Component", typeof(ComponentTarget)).ToMember();
        }

        public override void because()
        {
            visitor.ProcessComponent(new ReferenceComponentMapping(ComponentType.Component, memberProperty, typeof(ComponentTarget), typeof(Target), null));
        }

        [Test]
        public void should_throw_an_ambiguous_component_reference_exception()
        {
            thrown_exception.ShouldNotBeNull();
            thrown_exception.ShouldBeOfType<AmbiguousComponentReferenceException>();
        }

        [Test]
        public void should_throw_exception_with_correct_message()
        {
            thrown_exception.Message.ShouldEqual("Multiple external components for 'ComponentTarget', referenced from property 'Component' of 'Target', unable to continue.");
        }

        [Test]
        public void should_have_type_for_missing_component_in_exception()
        {
            ((AmbiguousComponentReferenceException)thrown_exception).ReferencedComponentType.ShouldEqual(typeof(ComponentTarget));
        }

        [Test]
        public void should_have_type_for_the_reference_in_exception()
        {
            ((AmbiguousComponentReferenceException)thrown_exception).SourceType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_have_property_from_the_reference_in_exception()
        {
            ((AmbiguousComponentReferenceException)thrown_exception).SourceMember.ShouldEqual(memberProperty);
        }
    }

    public abstract class ComponentReferenceResolutionVisitorSpec : Specification
    {
        protected ComponentReferenceResolutionVisitor visitor;
        protected Member memberProperty;

        protected class ComponentTarget
        { }

        protected class Target { }
    }
}
