using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default entity table name convention
    /// </summary>
    public class TableNameConvention : IClassConvention
    {
        public bool Accept(IClassMap classMap)
        {
            return string.IsNullOrEmpty(classMap.TableName);
        }

        public void Apply(IClassMap classMap)
        {
            classMap.WithTable("`" + classMap.EntityType.Name + "`");
        }
    }
}