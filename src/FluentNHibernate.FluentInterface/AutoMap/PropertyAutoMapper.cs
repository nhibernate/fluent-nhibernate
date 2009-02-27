using FluentNHibernate.MappingModel;

namespace FluentNHibernate.FluentInterface.AutoMap
{
    public class PropertyAutoMapper : IAutoMapper
    {
        public void Map(ClassMapping classMap)
        {
            foreach(var property in classMap.Type.GetProperties())
            {
                if (!classMap.HasMappedProperty(property))
                {
                    if (property.PropertyType.Namespace == "System")
                        classMap.AddProperty(new PropertyMapping
                            {
                                PropertyInfo = property,
                                Name = property.Name,
                            });
                }
            }
        }
    }
}