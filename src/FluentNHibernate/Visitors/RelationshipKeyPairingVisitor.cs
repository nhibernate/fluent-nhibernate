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
            var otherSide = (ICollectionMapping)thisSide.OtherSide;

            // both sides have user-defined key columns... leave alone!
            if (otherSide.Key.Columns.HasUserDefined() && thisSide.Columns.HasUserDefined())
                return;

            if (otherSide.Key.Columns.HasUserDefined())
            {
                // only other side has user-defined columns, so we'll bring them across to this side
                thisSide.ClearColumns();
                otherSide.Key.Columns.Each(x => thisSide.AddColumn(x.Clone()));
                return;
            }

            if (otherSide.Key.Columns.HasUserDefined())
            {
                // only other side has user-defined columns, so we'll bring them across to this side
                thisSide.ClearColumns();
                otherSide.Key.Columns.Each(x => thisSide.AddColumn(x.Clone()));
                return;
            }

            // the other side doesn't have any user defined columns, so we'll use the ones from this side
            // whether they're user defined or not.
            otherSide.Key.ClearColumns();
            thisSide.Columns.Each(x => otherSide.Key.AddColumn(x.Clone()));
        }
    }
}