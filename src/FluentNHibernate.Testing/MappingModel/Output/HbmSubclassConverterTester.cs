using System.Collections.Generic;
using FakeItEasy;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmSubclassConverterTester
    {
        // No use for the normal setup method to populate the container, since no test uses it

        [Test]
        public void ShouldProcessSubclassAsBasicSubclass()
        {
            ShouldConvertSpecificHbmForMappingSubtype<SubclassMapping, object, HbmSubclass>(
                () => new SubclassMapping(SubclassType.Subclass)
            );
        }

        [Test]
        public void ShouldProcessJoinedSubclassAsJoinedSubclass()
        {
            ShouldConvertSpecificHbmForMappingSubtype<SubclassMapping, object, HbmJoinedSubclass>(
                () => new SubclassMapping(SubclassType.JoinedSubclass)
            );
        }

        [Test]
        public void ShouldProcessUnionSubclassAsUnionSubclass()
        {
            ShouldConvertSpecificHbmForMappingSubtype<SubclassMapping, object, HbmUnionSubclass>(
                () => new SubclassMapping(SubclassType.UnionSubclass)
            );
        }

        /* Unfortunately, there is no good way to test the "fail if passed something unsupported" logic, because we cannot
         * generate a "bad" SubclassType and we want to support all of the real ones.
         */
    }
}
