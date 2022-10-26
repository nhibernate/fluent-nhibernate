using System.Collections.Generic;
using FakeItEasy;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmComponentBasedConverterTester
    {
        [Test]
        public void ShouldConvertComponentAsComponent()
        {
            ShouldConvertSpecificHbmForMapping<IComponentMapping, ComponentMapping, object, HbmComponent>(
                () => new ComponentMapping(ComponentType.Component)
            );
        }

        [Test]
        public void ShouldConvertDynamicComponentAsDynamicComponent()
        {
            ShouldConvertSpecificHbmForMapping<IComponentMapping, ComponentMapping, object, HbmDynamicComponent>(
                () => new ComponentMapping(ComponentType.DynamicComponent)
            );
        }

        [Test]
        public void ShouldConvertExternalComponentAsComponent()
        {
            ShouldConvertSpecificHbmForMapping<IComponentMapping, ComponentMapping, object, HbmComponent>(
                () => new ExternalComponentMapping(ComponentType.Component)
            );
        }

        [Test]
        public void ShouldConvertDynamicExternalComponentAsDynamicComponent()
        {
            ShouldConvertSpecificHbmForMapping<IComponentMapping, ComponentMapping, object, HbmDynamicComponent>(
                () => new ExternalComponentMapping(ComponentType.DynamicComponent)
            );
        }

        /* Unfortunately, there is no good way to test the "fail if passed something unsupported" logic, because we cannot
         * generate a "bad" ComponentType and we want to support all of the real ones.
         */

        [Test]
        public void ShouldConvertReferenceComponentAsComponent()
        {
            /* This test requires somewhat specialized setup. Specifically, rather than looking for a handoff of the entire
             * mapping to an IHbmConverter<ReferenceComponentMapping, X> for some specific X, we look for a handoff of the merged
             * model to an IHbmConverter<IComponentMapping, object>. This is due to how the existing code structures things.
             */

            // Set up a fake converter that registers any instances it generates and returns in a list
            var generatedHbms = new List<object>();
            var fakeConverter = A.Fake<IHbmConverter<IComponentMapping, object>>();
            A.CallTo(() => fakeConverter.Convert(A<IComponentMapping>.Ignored)).ReturnsLazily(fSub =>
            {
                var hbm = new HbmComponent();
                generatedHbms.Add(hbm);
                return hbm;
            });

            // Set up a custom container with the fake IComponentMapping->object converter registered, and obtain our main
            // converter from it (so that it will use the fake implementation). Note that we do the resolution _before_ we
            // register the fake, so that in cases where we are doing recursive types and HSuper == H we get the real converter
            // for the "outer" call but the fake for any "inner" calls.
            var container = new HbmConverterContainer();
            IHbmConverter<ReferenceComponentMapping, object> converter = container.Resolve<IHbmConverter<ReferenceComponentMapping, object>>();
            container.Register<IHbmConverter<IComponentMapping, object>>(cnvrt => fakeConverter);

            // Allocate an instance of the descendant type, but explicitly label it as the ancestor type to ensure that we pass it correctly
            ReferenceComponentMapping mapping = CreateReferenceComponentInstance();

            // Now try to convert it
            var convertedHbm = converter.Convert(mapping);

            // Check that the expected converter was invoked *with the merged model* the correct number of times and that the returned value is the converted instance
            A.CallTo(() => fakeConverter.Convert(mapping.MergedModel)).MustHaveHappened(Repeated.Exactly.Once); // Do this first since it guarantees the list should have exactly one item
            convertedHbm.ShouldEqual(generatedHbms[0]);
        }

        // NOTE: Because reference components require the target component type, they don't make any sense as dynamic components
        // (and I'm not certain why the constructor even allows for it; nothing in the code base appears to use it). Therefore,
        // we don't currently test for it here. If a well-defined use case is added later, a matching test should be added here.

        private ReferenceComponentMapping CreateReferenceComponentInstance()
        {
            var property = new DummyPropertyInfo("ComponentProperty", typeof(ComponentTarget)).ToMember();
            var instance = new ReferenceComponentMapping(ComponentType.Component, property, typeof(ComponentTarget), typeof(Target), null);
            instance.AssociateExternalMapping(new ExternalComponentMapping(ComponentType.Component));
            return instance;
        }

        private class Target
        {
            public int Id { get; set; }
            public ComponentTarget ComponentProperty { get; set; }
        }

        private class ComponentTarget
        {
            public string StringProperty { get; set; }
        }
    }
}