using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmCollectionConverterTester
    {
        // No use for the normal setup method to populate the container, since no test uses it

        [Test]
        public void ShouldProcessBagAsBag()
        {
            ShouldConvertSpecificHbmForMappingSubtype<CollectionMapping, object, HbmBag>(
                () => CollectionMapping.Bag()
            );
        }

        [Test]
        public void ShouldProcessListAsList()
        {
            ShouldConvertSpecificHbmForMappingSubtype<CollectionMapping, object, HbmList>(
                () => CollectionMapping.List()
            );
        }

        [Test]
        public void ShouldProcessSetAsSet()
        {
            ShouldConvertSpecificHbmForMappingSubtype<CollectionMapping, object, HbmSet>(
                () => CollectionMapping.Set()
            );
        }

        [Test]
        public void ShouldProcessMapAsMap()
        {
            ShouldConvertSpecificHbmForMappingSubtype<CollectionMapping, object, HbmMap>(
                () => CollectionMapping.Map()
            );
        }

        [Test]
        public void ShouldProcessArrayAsArray()
        {
            ShouldConvertSpecificHbmForMappingSubtype<CollectionMapping, object, HbmArray>(
                () => CollectionMapping.Array()
            );
        }

        /* Unfortunately, there is no good way to test the "fail if passed something unsupported" logic, because we cannot
         * generate a "bad" Collection type (it is an enum) and we want to support all of the real ones.
         */
    }
}
