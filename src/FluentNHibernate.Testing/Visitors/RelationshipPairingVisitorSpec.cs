using System.Linq;
using FakeItEasy;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Visitors
{
    public abstract class RelationshipPairingVisitorSpec : Specification
    {
        protected RelationshipPairingVisitor visitor;
        protected CollectionMapping collectionMappingToZ;
        protected ManyToOneMapping manyToOneZToHolder;
        protected ManyToOneMapping manyToOneYToHolder;
    }

    [TestFixture]
    public class when_the_relationship_pairing_visitor_visits_with_multiple_many_to_one_mapping_references : RelationshipPairingVisitorSpec
    {
        public override void establish_context()
        {
            manyToOneYToHolder = new ManyToOneMapping();
            manyToOneYToHolder.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(Holder)));
            manyToOneYToHolder.Set(x => x.Name, Layer.Defaults, "ARankedFirstProperty");
            manyToOneYToHolder.ContainingEntityType = typeof(ARankedFirst);
            manyToOneYToHolder.Member = new PropertyMember(typeof(ARankedFirst).GetProperty("ARankedFirstProperty"));

            manyToOneZToHolder = new ManyToOneMapping();
            manyToOneZToHolder.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(Holder)));
            manyToOneZToHolder.Set(x => x.Name, Layer.Defaults, "BRankedSecondProperty");
            manyToOneZToHolder.ContainingEntityType = typeof(BRankedSecond);
            manyToOneZToHolder.Member = new PropertyMember(typeof(BRankedSecond).GetProperty("BRankedSecondProperty"));

            var relationship = new OneToManyMapping();
            relationship.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(BRankedSecond)));
            relationship.ContainingEntityType = typeof(Holder);

            collectionMappingToZ = CollectionMapping.Bag();
            collectionMappingToZ.Set(x => x.ChildType, Layer.Defaults, typeof(BRankedSecond));
            collectionMappingToZ.Set(x => x.Name, Layer.Defaults, "ColectionOfBRankedSeconds");
            collectionMappingToZ.Set(x => x.Relationship, Layer.Defaults, relationship);
            collectionMappingToZ.ContainingEntityType = typeof(Holder);

            visitor = new RelationshipPairingVisitor(A.Fake<PairBiDirectionalManyToManySidesDelegate>());
        }

        [Test]
        public void should_associate_the_collection_mapping_to_the_correct_type()
        {
            visitor.ProcessCollection(collectionMappingToZ);
            visitor.ProcessManyToOne(manyToOneYToHolder);
            visitor.ProcessManyToOne(manyToOneZToHolder);
            visitor.Visit(Enumerable.Empty<HibernateMapping>());

            var otherSide = (ManyToOneMapping)collectionMappingToZ.OtherSide;
            otherSide.ContainingEntityType.ShouldEqual(typeof(BRankedSecond));
        }
    }
}