using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
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

        public IClassInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public void Table(string tableName)
        {
            if (!mapping.IsSpecified(x => x.TableName))
                mapping.TableName = tableName;
        }

        public new void DynamicInsert()
        {
            if (!mapping.IsSpecified(x => x.DynamicInsert))
            {
                mapping.DynamicInsert = nextBool;
                nextBool = true;
            }
        }

        public new void DynamicUpdate()
        {
            if (!mapping.IsSpecified(x => x.DynamicUpdate))
            {
                mapping.DynamicUpdate = nextBool;
                nextBool = true;
            }
        }

        public new IOptimisticLockInstance OptimisticLock
        {
            get
            {
                return new OptimisticLockInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.OptimisticLock))
                        mapping.OptimisticLock = value;
                });
            }
        }

        public new void BatchSize(int size)
        {
            if (!mapping.IsSpecified(x => x.BatchSize))
                mapping.BatchSize = size;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified(x => x.Lazy))
            {
                mapping.Lazy = nextBool;
                nextBool = true;
            }
        }

        public new void ReadOnly()
        {
            if (!mapping.IsSpecified(x => x.Mutable))
            {
                mapping.Mutable = !nextBool;
                nextBool = true;
            }
        }

        public new void Schema(string schema)
        {
            if (!mapping.IsSpecified(x => x.Schema))
                mapping.Schema = schema;
        }

        public new void Where(string where)
        {
            if (!mapping.IsSpecified(x => x.Where))
                mapping.Where = where;
        }

        public new void Subselect(string subselectSql)
        {
            if (!mapping.IsSpecified(x => x.Subselect))
                mapping.Subselect = subselectSql;
        }
    }
}