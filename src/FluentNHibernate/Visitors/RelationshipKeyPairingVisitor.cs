using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Visitors;

public class RelationshipKeyPairingVisitor : DefaultMappingModelVisitor
{
    public override void ProcessManyToOne(ManyToOneMapping thisSide)
    {
        if (thisSide.OtherSide is null)
            return;

        // other side is always going to be a collection for a many-to-one mapping
        var otherSide = (CollectionMapping)thisSide.OtherSide;

        otherSide.Key.MakeColumnsEmpty(Layer.Defaults);
        foreach (var column in thisSide.Columns)
            otherSide.Key.AddColumn(Layer.Defaults, column.Clone());
    }
}
