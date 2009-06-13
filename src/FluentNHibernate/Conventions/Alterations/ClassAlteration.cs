using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Alterations
{
    public class ClassAlteration : IClassAlteration
    {
        private readonly ClassMapping mapping;

        public ClassAlteration(ClassMapping mapping)
        {
            this.mapping = mapping;
        }

        public void WithTable(string tableName)
        {
            mapping.TableName = tableName;
        }
    }
}