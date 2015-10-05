using System.Linq;
using FakeItEasy;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Visitors
{
    [TestFixture]
    public class when_the_relationship_pairing_visitor_visits_with_multiple_many_to_one_mapping_references : Specification
    {
        protected RelationshipPairingVisitor visitor;
        protected CollectionMapping collectionMappingToBRankedSecond;
        protected ManyToOneMapping manyToOneBRankedSecondToHolder;
        protected ManyToOneMapping manyToOneARankedFirstToHolder;

        public override void establish_context()
        {
            manyToOneARankedFirstToHolder = new ManyToOneMapping();
            manyToOneARankedFirstToHolder.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(Holder)));
            manyToOneARankedFirstToHolder.Set(x => x.Name, Layer.Defaults, "ARankedFirstProperty");
            manyToOneARankedFirstToHolder.ContainingEntityType = typeof(ARankedFirst);
            manyToOneARankedFirstToHolder.Member = new PropertyMember(typeof(ARankedFirst).GetProperty("ARankedFirstProperty"));

            manyToOneBRankedSecondToHolder = new ManyToOneMapping();
            manyToOneBRankedSecondToHolder.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(Holder)));
            manyToOneBRankedSecondToHolder.Set(x => x.Name, Layer.Defaults, "BRankedSecondProperty");
            manyToOneBRankedSecondToHolder.ContainingEntityType = typeof(BRankedSecond);
            manyToOneBRankedSecondToHolder.Member = new PropertyMember(typeof(BRankedSecond).GetProperty("BRankedSecondProperty"));

            var relationship = new OneToManyMapping();
            relationship.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(BRankedSecond)));
            relationship.ContainingEntityType = typeof(Holder);

            collectionMappingToBRankedSecond = CollectionMapping.Bag();
            collectionMappingToBRankedSecond.Set(x => x.ChildType, Layer.Defaults, typeof(BRankedSecond));
            collectionMappingToBRankedSecond.Set(x => x.Name, Layer.Defaults, "ColectionOfBRankedSeconds");
            collectionMappingToBRankedSecond.Set(x => x.Relationship, Layer.Defaults, relationship);
            collectionMappingToBRankedSecond.ContainingEntityType = typeof(Holder);

            visitor = new RelationshipPairingVisitor(A.Fake<PairBiDirectionalManyToManySidesDelegate>());
        }

        [Test]
        public void should_associate_the_collection_mapping_to_the_correct_type()
        {
            visitor.ProcessCollection(collectionMappingToBRankedSecond);
            visitor.ProcessManyToOne(manyToOneARankedFirstToHolder);
            visitor.ProcessManyToOne(manyToOneBRankedSecondToHolder);
            visitor.Visit(Enumerable.Empty<HibernateMapping>());

            var otherSide = (ManyToOneMapping)collectionMappingToBRankedSecond.OtherSide;
            otherSide.ContainingEntityType.ShouldEqual(typeof(BRankedSecond));
        }
    }
}
