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
                mapping.SetDefaultValue(x => x.TableName, mapping.ChildType.Name + "To" + mapping.ContainingEntityType.Name);
            }
            else
            {
                var otherSide = (MappingModel.Collections.CollectionMapping)mapping.OtherSide;

                // bi-directional
                if (mapping.IsSpecified("TableName") && otherSide.IsSpecified("TableName"))
                {
                    // TODO: We could check if they're the same here and warn the user if they're not
                    return;
                }

                if (mapping.IsSpecified("TableName") && !otherSide.IsSpecified("TableName"))
                    otherSide.SetDefaultValue(x => x.TableName, mapping.TableName);
                else if (!mapping.IsSpecified("TableName") && otherSide.IsSpecified("TableName"))
                    mapping.SetDefaultValue(x => x.TableName, otherSide.TableName);
                else
                {
                    var tableName = mapping.Member.Name + "To" + otherSide.Member.Name;

                    mapping.SetDefaultValue(x => x.TableName, tableName);
                    otherSide.SetDefaultValue(x => x.TableName, tableName);
                }
            }
        }
    }
}