using System;
using System.Diagnostics;
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IClassInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new ISchemaActionInstance SchemaAction
        {
            get
            {
                return new SchemaActionInstance(value =>
                {
                    if (!mapping.IsSpecified("SchemaAction"))
                        mapping.SchemaAction = value;
                });
            }
        }

        public void Table(string tableName)
        {
            if (!mapping.IsSpecified("TableName"))
                mapping.TableName = tableName;
        }

        public new void DynamicInsert()
        {
            if (!mapping.IsSpecified("DynamicInsert"))
            {
                mapping.DynamicInsert = nextBool;
                nextBool = true;
            }
        }

        public new void DynamicUpdate()
        {
            if (!mapping.IsSpecified("DynamicUpdate"))
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
                    if (!mapping.IsSpecified("OptimisticLock"))
                        mapping.OptimisticLock = value;
                });
            }
        }

        public new void BatchSize(int size)
        {
            if (!mapping.IsSpecified("BatchSize"))
                mapping.BatchSize = size;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
            {
                mapping.Lazy = nextBool;
                nextBool = true;
            }
        }

        public new void ReadOnly()
        {
            if (!mapping.IsSpecified("Mutable"))
            {
                mapping.Mutable = !nextBool;
                nextBool = true;
            }
        }

        public new void Schema(string schema)
        {
            if (!mapping.IsSpecified("Schema"))
                mapping.Schema = schema;
        }

        public new void Where(string where)
        {
            if (!mapping.IsSpecified("Where"))
                mapping.Where = where;
        }

        public new void Subselect(string subselectSql)
        {
            if (!mapping.IsSpecified("Subselect"))
                mapping.Subselect = subselectSql;
        }

        public new void Proxy<T>()
        {
            Proxy(typeof(T));
        }

        public new void Proxy(Type type)
        {
            if (!mapping.IsSpecified("Proxy"))
                mapping.Proxy = type.AssemblyQualifiedName;
        }

        public new void Proxy(string type)
        {
            if (!mapping.IsSpecified("Proxy"))
                mapping.Proxy = type;
        }
    }
}