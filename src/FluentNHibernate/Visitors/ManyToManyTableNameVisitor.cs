using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Visitors
{
    public class ManyToManyTableNameVisitor : DefaultMappingModelVisitor
    {
        public override void ProcessCollection(CollectionMapping mapping)
        {
            if (!(mapping.Relationship is ManyToManyMapping))
                return;

            if (mapping.OtherSide == null)
            {
                // uni-directional
                mapping.Set(x => x.TableName, Layer.Defaults, mapping.ChildType.Name + "To" + mapping.ContainingEntityType.Name);
            }
            else
            {
                var otherSide = (CollectionMapping)mapping.OtherSide;
                var tableName = mapping.TableName ?? otherSide.TableName ?? otherSide.Member.Name + "To" + mapping.Member.Name;

                mapping.Set(x => x.TableName, Layer.Defaults, tableName);
                otherSide.Set(x => x.TableName, Layer.Defaults, tableName);
            }
        }
    }
}