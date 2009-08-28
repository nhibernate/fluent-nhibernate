using System;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Conventions.Instances
{
    public class JoinedSubclassInstance : JoinedSubclassInspector, IJoinedSubclassInstance
    {
        private readonly JoinedSubclassMapping mapping;
        private bool nextBool = true;

        public JoinedSubclassInstance(JoinedSubclassMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IKeyInstance Key
        {
            get
            {
                if (mapping.Key == null)
                    mapping.Key = new KeyMapping();

                return new KeyInstance(mapping.Key);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IJoinedSubclassInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new void Abstract()
        {
            if (mapping.IsSpecified("Abstract"))
                return;

            mapping.Abstract = nextBool;
            nextBool = true;
        }

        public void Check(string constraint)
        {
            if (!mapping.IsSpecified("Check"))
                mapping.Check = constraint;
        }

        public new void DynamicInsert()
        {
            if (mapping.IsSpecified("DynamicInsert"))
                return;

            mapping.DynamicInsert = nextBool;
            nextBool = true;
        }

        public new void DynamicUpdate()
        {
            if (mapping.IsSpecified("DynamicUpdate"))
                return;

            mapping.DynamicUpdate = nextBool;
            nextBool = true;
        }

        public new void LazyLoad()
        {
            if (mapping.IsSpecified("Lazy"))
                return;

            mapping.Lazy = nextBool;
            nextBool = true;
        }

        public new void Proxy(Type type)
        {
            if (!mapping.IsSpecified("Proxy"))
                mapping.Proxy = type.AssemblyQualifiedName;
        }

        public new void Proxy<T>()
        {
            if (!mapping.IsSpecified("Proxy"))
                mapping.Proxy = typeof(T).AssemblyQualifiedName;
        }

        public void Schema(string schema)
        {
            if (!mapping.IsSpecified("Schema"))
                mapping.Schema = schema;
        }

        public new void SelectBeforeUpdate()
        {
            if (mapping.IsSpecified("SelectBeforeUpdate"))
                return;

            mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
        }

        public void Table(string tableName)
        {
            if (!mapping.IsSpecified("TableName"))
                mapping.TableName = tableName;
        }

        public void Subselect(string subselect)
        {
            if (!mapping.IsSpecified("Subselect"))
                mapping.Subselect = subselect;
        }

        public void Persister<T>() where T : IEntityPersister
        {
            if (!mapping.IsSpecified("Persister"))
                mapping.Persister = new TypeReference(typeof(T));
        }

        public void Persister(Type type)
        {
            if (!mapping.IsSpecified("Persister"))
                mapping.Persister = new TypeReference(type);
        }

        public void Persister(string type)
        {
            if (!mapping.IsSpecified("Persister"))
                mapping.Persister = new TypeReference(type);
        }

        public void BatchSize(int batchSize)
        {
            if (!mapping.IsSpecified("BatchSize"))
                mapping.BatchSize = batchSize;
        }
    }
}