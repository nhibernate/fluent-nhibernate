using System.Collections.Generic;
using FakeItEasy;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmSubclassConverterTester
    {
        // No use for the normal setup method to populate the container, since every test overrides it anyway

        [Test]
        public void ShouldProcessSubclassAsBasicSubclass()
        {
            // Define a fake converter for the desired mapping that stores its returned values in a list
            var fakeDesiredConverter = A.Fake<IHbmConverter<SubclassMapping, HbmSubclass>>();
            var generatedHbms = new List<HbmSubclass>();
            A.CallTo(() => fakeDesiredConverter.Convert(A<SubclassMapping>.Ignored)).ReturnsLazily(ignored =>
            {
                var hbm = new HbmSubclass();
                generatedHbms.Add(hbm);
                return hbm;
            });

            // Define fake converters for all of the non-desired mappings
            var fakeUndesiredConverter1 = A.Fake<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>();
            var fakeUndesiredConverter2 = A.Fake<IHbmConverter<SubclassMapping, HbmUnionSubclass>>();

            // Set up a custom container with all of the fake converters registered, and obtain our main converter from it (so
            // that it will use the fake implementation)
            var container = new HbmConverterContainer();
            container.Register<IHbmConverter<SubclassMapping, HbmSubclass>>(cnvrt => fakeDesiredConverter);
            container.Register<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>(cnvrt => fakeUndesiredConverter1);
            container.Register<IHbmConverter<SubclassMapping, HbmUnionSubclass>>(cnvrt => fakeUndesiredConverter2);
            IHbmConverter<SubclassMapping, object> converter = container.Resolve<IHbmConverter<SubclassMapping, object>>();

            // Set up a SubclassMapping of the appropriate subtype and convert it
            var subclassMapping = new SubclassMapping(SubclassType.Subclass);
            var convertedSubclass = converter.Convert(subclassMapping);

            // Assert that the desired converter got called exactly once
            A.CallTo(() => fakeDesiredConverter.Convert(A<SubclassMapping>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

            // Assert that the two incorrect converters were never called
            A.CallTo(() => fakeUndesiredConverter1.Convert(A<SubclassMapping>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeUndesiredConverter2.Convert(A<SubclassMapping>.Ignored)).MustNotHaveHappened();

            // Assert that the returned value is the one that was generated
            convertedSubclass.ShouldEqual(generatedHbms[0]);
        }

        [Test]
        public void ShouldProcessJoinedSubclassAsJoinedSubclass()
        {
            // Define a fake converter for the desired mapping that stores its returned values in a list
            var fakeDesiredConverter = A.Fake<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>();
            var generatedHbms = new List<HbmJoinedSubclass>();
            A.CallTo(() => fakeDesiredConverter.Convert(A<SubclassMapping>.Ignored)).ReturnsLazily(ignored =>
            {
                var hbm = new HbmJoinedSubclass();
                generatedHbms.Add(hbm);
                return hbm;
            });

            // Define fake converters for all of the non-desired mappings
            var fakeUndesiredConverter1 = A.Fake<IHbmConverter<SubclassMapping, HbmSubclass>>();
            var fakeUndesiredConverter2 = A.Fake<IHbmConverter<SubclassMapping, HbmUnionSubclass>>();

            // Set up a custom container with all of the fake converters registered, and obtain our main converter from it (so
            // that it will use the fake implementation)
            var container = new HbmConverterContainer();
            container.Register<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>(cnvrt => fakeDesiredConverter);
            container.Register<IHbmConverter<SubclassMapping, HbmSubclass>>(cnvrt => fakeUndesiredConverter1);
            container.Register<IHbmConverter<SubclassMapping, HbmUnionSubclass>>(cnvrt => fakeUndesiredConverter2);
            IHbmConverter<SubclassMapping, object> converter = container.Resolve<IHbmConverter<SubclassMapping, object>>();

            // Set up a SubclassMapping of the appropriate subtype and convert it
            var subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass);
            var convertedSubclass = converter.Convert(subclassMapping);

            // Assert that the desired converter got called exactly once
            A.CallTo(() => fakeDesiredConverter.Convert(A<SubclassMapping>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

            // Assert that the two incorrect converters were never called
            A.CallTo(() => fakeUndesiredConverter1.Convert(A<SubclassMapping>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeUndesiredConverter2.Convert(A<SubclassMapping>.Ignored)).MustNotHaveHappened();

            // Assert that the returned value is the one that was generated
            convertedSubclass.ShouldEqual(generatedHbms[0]);
        }

        [Test]
        public void ShouldProcessUnionSubclassAsUnionSubclass()
        {
            // Define a fake converter for the desired mapping that stores its returned values in a list
            var fakeDesiredConverter = A.Fake<IHbmConverter<SubclassMapping, HbmUnionSubclass>>();
            var generatedHbms = new List<HbmUnionSubclass>();
            A.CallTo(() => fakeDesiredConverter.Convert(A<SubclassMapping>.Ignored)).ReturnsLazily(ignored =>
            {
                var hbm = new HbmUnionSubclass();
                generatedHbms.Add(hbm);
                return hbm;
            });

            // Define fake converters for all of the non-desired mappings
            var fakeUndesiredConverter1 = A.Fake<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>();
            var fakeUndesiredConverter2 = A.Fake<IHbmConverter<SubclassMapping, HbmSubclass>>();

            // Set up a custom container with all of the fake converters registered, and obtain our main converter from it (so
            // that it will use the fake implementation)
            var container = new HbmConverterContainer();
            container.Register<IHbmConverter<SubclassMapping, HbmUnionSubclass>>(cnvrt => fakeDesiredConverter);
            container.Register<IHbmConverter<SubclassMapping, HbmJoinedSubclass>>(cnvrt => fakeUndesiredConverter1);
            container.Register<IHbmConverter<SubclassMapping, HbmSubclass>>(cnvrt => fakeUndesiredConverter2);
            IHbmConverter<SubclassMapping, object> converter = container.Resolve<IHbmConverter<SubclassMapping, object>>();

            // Set up a SubclassMapping of the appropriate subtype and convert it
            var subclassMapping = new SubclassMapping(SubclassType.UnionSubclass);
            var convertedSubclass = converter.Convert(subclassMapping);

            // Assert that the desired converter got called exactly once
            A.CallTo(() => fakeDesiredConverter.Convert(A<SubclassMapping>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

            // Assert that the two incorrect converters were never called
            A.CallTo(() => fakeUndesiredConverter1.Convert(A<SubclassMapping>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeUndesiredConverter2.Convert(A<SubclassMapping>.Ignored)).MustNotHaveHappened();

            // Assert that the returned value is the one that was generated
            convertedSubclass.ShouldEqual(generatedHbms[0]);
        }

        /* Unfortunately, there is no good way to test the "fail if passed something unsupported" logic, because we cannot
         * generate a "bad" SubclassType and we want to support all of the real ones.
         */
    }
}
