using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using static FluentNHibernate.Testing.Hbm.HbmConverterTestHelper;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmCollectionRelationshipConverterTester
    {
        // No use for the normal setup method to populate the container, since no test uses it

        [Test]
        public void ShouldProcessOneToManyAsOneToMany()
        {
            ShouldConvertSpecificHbmForMappingChild<ICollectionRelationshipMapping, OneToManyMapping, object, HbmOneToMany>();
        }

        [Test]
        public void ShouldProcessManyToManyAsManyToMany()
        {
            ShouldConvertSpecificHbmForMappingChild<ICollectionRelationshipMapping, ManyToManyMapping, object, HbmManyToMany>();
        }
    }
}
