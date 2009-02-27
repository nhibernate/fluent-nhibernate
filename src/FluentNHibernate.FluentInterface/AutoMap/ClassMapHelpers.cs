using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.FluentInterface.AutoMap
{
    /// <summary>
    /// In a static class as I wasnt sure if I should add this to the semantic model.
    /// </summary>
    public static class ClassMapHelpers
    {
        public static bool HasMappedProperty(this ClassMapping classMap, PropertyInfo propertyInfo)
        {
            if (classMap.Id != null)
            {
                foreach (var column in (classMap.Id as IdMapping).Columns)
                {
                    if (column.PropertyInfo == propertyInfo)
                        return true;
                }
            }

            foreach (var propertyMapping in classMap.Properties)
            {
                if (propertyMapping.PropertyInfo == propertyInfo)
                    return true;
            }

            return false;
        }

    }
}