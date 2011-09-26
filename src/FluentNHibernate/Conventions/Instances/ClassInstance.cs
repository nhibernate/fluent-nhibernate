using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
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
            get { return new SchemaActionInstance(value => mapping.Set(x => x.SchemaAction, Layer.Conventions, value)); }
        }

        public void Table(string tableName)
        {
            mapping.Set(x => x.TableName, Layer.Conventions, tableName);
        }

        public new void DynamicInsert()
        {
            mapping.Set(x => x.DynamicInsert, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void DynamicUpdate()
        {
            mapping.Set(x => x.DynamicUpdate, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new IOptimisticLockInstance OptimisticLock
        {
            get { return new OptimisticLockInstance(value => mapping.Set(x => x.OptimisticLock, Layer.Conventions, value)); }
        }

        public new void BatchSize(int size)
        {
            mapping.Set(x => x.BatchSize, Layer.Conventions, size);
        }

        public new void LazyLoad()
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void ReadOnly()
        {
            mapping.Set(x => x.Mutable, Layer.Conventions, !nextBool);
            nextBool = true;
        }

        public new void Schema(string schema)
        {
            mapping.Set(x => x.Schema, Layer.Conventions, schema);
        }

        public new void Where(string where)
        {
            mapping.Set(x => x.Where, Layer.Conventions, where);
        }

        public new void Subselect(string subselectSql)
        {
            mapping.Set(x => x.Subselect, Layer.Conventions, subselectSql);
        }

        public new void Proxy<T>()
        {
            Proxy(typeof(T));
        }

        public new void Proxy(Type type)
        {
            Proxy(type.AssemblyQualifiedName);
        }

        public new void Proxy(string type)
        {
            mapping.Set(x => x.Proxy, Layer.Conventions, type);
        }

        public void ApplyFilter(string name, string condition)
        {
            var filterMapping = new FilterMapping();
            filterMapping.Set(x => x.Name, Layer.Conventions, name);
            filterMapping.Set(x => x.Condition, Layer.Conventions, condition);
            mapping.AddFilter(filterMapping);
        }

        public void ApplyFilter(string name)
        {
            ApplyFilter(name, null);
        }

        public void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
        {
            ApplyFilter(new TFilter().Name, condition);
        }

        public void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
        {
            ApplyFilter<TFilter>(null);
        }
    }
}