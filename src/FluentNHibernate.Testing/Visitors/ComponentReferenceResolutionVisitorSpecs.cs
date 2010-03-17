using FluentNHibernate.Mapping.Providers;
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
            external_component_mapping = new ExternalComponentMapping(ComponentType.Component) { Type = typeof(ComponentTarget) };
            var external_component = Stub<IExternalComponentMappingProvider>.Create(cfg =>
                cfg.Stub(x => x.GetComponentMapping()).Return(external_component_mapping));

            visitor = new ComponentReferenceResolutionVisitor(new[] { external_component });
            reference_component_mapping = new ReferenceComponentMapping(ComponentType.Component, null, null, null, null);
        }

        public override void because()
        {
            visitor.ProcessComponent(reference_component_mapping);
        }

        [Test]
        public void should_associate_external_mapping_with_reference_mapping()
        {
            reference_component_mapping.IsAssociated.ShouldBeTrue();
        }

        private ReferenceComponentMapping reference_component_mapping;
        private ExternalComponentMapping external_component_mapping;
    }

    [TestFixture]
    public class when_the_component_reference_resolution_visitor_processes_a_component_reference_without_a_corresponding_external_component : ComponentReferenceResolutionVisitorSpec
    {
        public override void establish_context()
        {
            visitor = new ComponentReferenceResolutionVisitor(new IExternalComponentMappingProvider[0]);
            member_property = new DummyPropertyInfo("Component", typeof(ComponentTarget)).ToMember();
        }

        public override void because()
        {
            visitor.ProcessComponent(new ReferenceComponentMapping(ComponentType.Component, member_property, typeof(ComponentTarget), typeof(Target), null));
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
            (thrown_exception as MissingExternalComponentException).ReferencedComponentType.ShouldEqual(typeof(ComponentTarget));
        }

        [Test]
        public void should_have_type_for_the_reference_in_exception()
        {
            (thrown_exception as MissingExternalComponentException).SourceType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_have_property_from_the_reference_in_exception()
        {
            (thrown_exception as MissingExternalComponentException).SourceMember.ShouldEqual(member_property);
        }
    }

    [TestFixture]
    public class when_the_component_reference_resolution_visitor_processes_a_component_reference_with_multiple_corresponding_external_components : ComponentReferenceResolutionVisitorSpec
    {
        public override void establish_context()
        {
            var external_component_one = Stub<IExternalComponentMappingProvider>.Create(cfg =>
            {
                cfg.Stub(x => x.GetComponentMapping()).Return(new ExternalComponentMapping(ComponentType.Component) { Type = typeof(ComponentTarget) });
                cfg.Stub(x => x.Type).Return(typeof(ComponentTarget));
            });
            var external_component_two = Stub<IExternalComponentMappingProvider>.Create(cfg =>
            {
                cfg.Stub(x => x.GetComponentMapping()).Return(new ExternalComponentMapping(ComponentType.Component) { Type = typeof(ComponentTarget) });
                cfg.Stub(x => x.Type).Return(typeof(ComponentTarget));
            });

            visitor = new ComponentReferenceResolutionVisitor(new[] { external_component_one, external_component_two});
            member_property = new DummyPropertyInfo("Component", typeof(ComponentTarget)).ToMember();
        }

        public override void because()
        {
            visitor.ProcessComponent(new ReferenceComponentMapping(ComponentType.Component, member_property, typeof(ComponentTarget), typeof(Target), null));
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
            (thrown_exception as AmbiguousComponentReferenceException).ReferencedComponentType.ShouldEqual(typeof(ComponentTarget));
        }

        [Test]
        public void should_have_type_for_the_reference_in_exception()
        {
            (thrown_exception as AmbiguousComponentReferenceException).SourceType.ShouldEqual(typeof(Target));
        }

        [Test]
        public void should_have_property_from_the_reference_in_exception()
        {
            (thrown_exception as AmbiguousComponentReferenceException).SourceMember.ShouldEqual(member_property);
        }
    }


    public abstract class ComponentReferenceResolutionVisitorSpec : Specification
    {
        protected ComponentReferenceResolutionVisitor visitor;
        protected Member member_property;

        protected class ComponentTarget
        { }

        protected class Target { }
    }
}
