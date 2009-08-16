using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public class ManyToManyTableNameVisitor : DefaultMappingModelVisitor
    {
        protected override void ProcessCollection(ICollectionMapping mapping)
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
                // bi-directional
                if (mapping.IsSpecified(x => x.TableName) && mapping.OtherSide.IsSpecified(x => x.TableName))
                {
                    // TODO: We could check if they're the same here and warn the user if they're not
                    return;
                }

                if (mapping.IsSpecified(x => x.TableName) && !mapping.OtherSide.IsSpecified(x => x.TableName))
                    mapping.OtherSide.SetDefaultValue(x => x.TableName, mapping.TableName);
                else if (!mapping.IsSpecified(x => x.TableName) && mapping.OtherSide.IsSpecified(x => x.TableName))
                    mapping.SetDefaultValue(x => x.TableName, mapping.OtherSide.TableName);
                else
                {
                    var tableName = mapping.MemberInfo.Name + "To" + mapping.OtherSide.MemberInfo.Name;

                    mapping.SetDefaultValue(x => x.TableName, tableName);
                    mapping.OtherSide.SetDefaultValue(x => x.TableName, tableName);
                }
            }
        }
    }
}