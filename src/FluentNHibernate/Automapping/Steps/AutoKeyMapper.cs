using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Automapping.Steps
{
    public class AutoKeyMapper
    {
        public void SetKey(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var columnName = property.DeclaringType.Name + "_id";
            var key = new KeyMapping
            {
                ContainingEntityType = classMap.Type
            };

            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, columnName);
            key.AddColumn(Layer.Defaults, columnMapping);

            mapping.Set(x => x.Key, Layer.Defaults, key);
        }
    }
}