using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    public class TableNameConvention : IClassConvention
    {
        public bool Accept(IClassMap classMap)
        {
            return string.IsNullOrEmpty(classMap.TableName);
        }

        public void Apply(IClassMap classMap, ConventionOverrides overrides)
        {
            if (overrides.GetTableName == null)
                classMap.WithTable("`" + classMap.EntityType.Name + "`");
            else
                classMap.WithTable(overrides.GetTableName(classMap.EntityType));
        }
    }
}