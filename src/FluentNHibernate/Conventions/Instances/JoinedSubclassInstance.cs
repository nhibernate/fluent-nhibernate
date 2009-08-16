using System;
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
            if (mapping.IsSpecified(x => x.Abstract))
                return;

            mapping.Abstract = nextBool;
            nextBool = true;
        }

        public void Check(string constraint)
        {
            if (!mapping.IsSpecified(x => x.Check))
                mapping.Check = constraint;
        }

        public new void DynamicInsert()
        {
            if (mapping.IsSpecified(x => x.DynamicInsert))
                return;

            mapping.DynamicInsert = nextBool;
            nextBool = true;
        }

        public new void DynamicUpdate()
        {
            if (mapping.IsSpecified(x => x.DynamicUpdate))
                return;

            mapping.DynamicUpdate = nextBool;
            nextBool = true;
        }

        public new void LazyLoad()
        {
            if (mapping.IsSpecified(x => x.Lazy))
                return;

            mapping.Lazy = nextBool;
            nextBool = true;
        }

        public new void Proxy(Type type)
        {
            if (!mapping.IsSpecified(x => x.Proxy))
                mapping.Proxy = type.AssemblyQualifiedName;
        }

        public new void Proxy<T>()
        {
            if (!mapping.IsSpecified(x => x.Proxy))
                mapping.Proxy = typeof(T).AssemblyQualifiedName;
        }

        public void Schema(string schema)
        {
            if (!mapping.IsSpecified(x => x.Schema))
                mapping.Schema = schema;
        }

        public new void SelectBeforeUpdate()
        {
            if (mapping.IsSpecified(x => x.SelectBeforeUpdate))
                return;

            mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
        }

        public void Table(string tableName)
        {
            if (!mapping.IsSpecified(x => x.TableName))
                mapping.TableName = tableName;
        }

        public void Subselect(string subselect)
        {
            if (!mapping.IsSpecified(x => x.Subselect))
                mapping.Subselect = subselect;
        }

        public void Persister<T>() where T : IEntityPersister
        {
            if (!mapping.IsSpecified(x => x.Persister))
                mapping.Persister = new TypeReference(typeof(T));
        }

        public void Persister(Type type)
        {
            if (!mapping.IsSpecified(x => x.Persister))
                mapping.Persister = new TypeReference(type);
        }

        public void Persister(string type)
        {
            if (!mapping.IsSpecified(x => x.Persister))
                mapping.Persister = new TypeReference(type);
        }

        public void BatchSize(int batchSize)
        {
            if (!mapping.IsSpecified(x => x.BatchSize))
                mapping.BatchSize = batchSize;
        }
    }
}