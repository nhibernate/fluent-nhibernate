using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances
{
    public class ClassInstance : ClassInspector, IClassInstance
    {
        private readonly ClassMapping mapping;
        private bool nextBool = true;

        public ClassInstance(ClassMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void WithTable(string tableName)
        {
            mapping.TableName = tableName;
        }

        public new void DynamicInsert()
        {
            mapping.DynamicInsert = nextBool;
            nextBool = true;
        }

        public new void DynamicUpdate()
        {
            mapping.DynamicUpdate = nextBool;
            nextBool = true;
        }

        public IClassInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new IOptimisticLockBuilder OptimisticLock
        {
            get { return new OptimisticLockBuilder(value => mapping.OptimisticLock = value); }
        }
    }
}