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

            otherSide.Key.MakeColumnsEmpty(Layer.Defaults);
            thisSide.Columns.Each(x => otherSide.Key.AddColumn(Layer.Defaults, x.Clone()));
        }
    }
}