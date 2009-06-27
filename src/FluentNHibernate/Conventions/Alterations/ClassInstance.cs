using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Alterations
{
    public class ClassInstance : ClassInspector, IClassInstance
    {
        private readonly ClassMapping mapping;

        public ClassInstance(ClassMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void WithTable(string tableName)
        {
            mapping.TableName = tableName;
        }
    }
}