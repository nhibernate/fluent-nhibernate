using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public class BiDirectionalManyToManyPairingVisitor : DefaultMappingModelVisitor
    {
        private readonly List<ICollectionMapping> relationships = new List<ICollectionMapping>();

        protected override void ProcessCollection(ICollectionMapping mapping)
        {
            if (!(mapping.Relationship is ManyToManyMapping))
                return;

            var potentialOtherSides = relationships
                .Where(x => x.ContainingEntityType == mapping.ChildType && x.ChildType == mapping.ContainingEntityType);

            if (potentialOtherSides.Count() == 1)
            {
                var otherSide = potentialOtherSides.First();

                mapping.OtherSide = otherSide;
                otherSide.OtherSide = mapping;
            }
            else if (potentialOtherSides.Count() > 1)
            {
                var reducedOtherSides = potentialOtherSides
                    .Where(x => x.MemberInfo.Name == mapping.MemberInfo.Name.Replace(mapping.ChildType.Name, x.ChildType.Name));

                if (reducedOtherSides.Count() != 1)
                    throw new NotSupportedException("Can't figure out what the other side of a many-to-many should be.");

                var otherSide = reducedOtherSides.First();

                mapping.OtherSide = otherSide;
                otherSide.OtherSide = mapping;
            }

            relationships.Add(mapping);
        }
    }
}