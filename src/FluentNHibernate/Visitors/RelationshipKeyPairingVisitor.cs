using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Visitors
{
    public class RelationshipKeyPairingVisitor : DefaultMappingModelVisitor
    {
        public override void ProcessManyToOne(ManyToOneMapping thisSide)
        {
            if (thisSide.OtherSide == null)
                return;

            // other side is always going to be a collection for a many-to-one mapping
            var otherSide = (CollectionMapping)thisSide.OtherSide;

            if (thisSide.ContainingEntityType == otherSide.ContainingEntityType)
            {
                // special case for self-referential relationships
                if (thisSide.Columns.HasUserDefined() || otherSide.Key.Columns.HasUserDefined())
                    return; // leave alone if user defined

                otherSide.Key.ClearColumns();
                thisSide.Columns.Each(x => otherSide.Key.AddDefaultColumn(x.Clone()));
            }
        }
    }
}