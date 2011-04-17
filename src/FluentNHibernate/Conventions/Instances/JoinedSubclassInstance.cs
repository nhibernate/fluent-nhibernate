using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Persister.Entity;

namespace FluentNHibernate.Conventions.Instances
{
    public class JoinedSubclassInstance : JoinedSubclassInspector, IJoinedSubclassInstance
    {
        private readonly SubclassMapping mapping;
        private bool nextBool = true;

        public JoinedSubclassInstance(SubclassMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IKeyInstance Key
        {
            get
            {
                if (mapping.Key == null)
                    mapping.Set(x => x.Key, Layer.Conventions, new KeyMapping());

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
            mapping.Set(x => x.Abstract, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Check(string constraint)
        {
            mapping.Set(x => x.Check, Layer.Conventions, constraint);
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

        public new void LazyLoad()
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void Proxy(Type type)
        {
            mapping.Set(x => x.Proxy, Layer.Conventions, type.AssemblyQualifiedName);
        }

        public new void Proxy<T>()
        {
            Proxy(typeof(T));
        }

        public void Schema(string schema)
        {
            mapping.Set(x => x.Schema, Layer.Conventions, schema);
        }

        public new void SelectBeforeUpdate()
        {
            mapping.Set(x => x.SelectBeforeUpdate, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public void Table(string tableName)
        {
            mapping.Set(x => x.TableName, Layer.Conventions, tableName);
        }

        public void Subselect(string subselect)
        {
            mapping.Set(x => x.Subselect, Layer.Conventions, subselect);
        }

        public void Persister<T>() where T : IEntityPersister
        {
            Persister(typeof(T));
        }

        public void Persister(Type type)
        {
            mapping.Set(x => x.Persister, Layer.Conventions, new TypeReference(type));
        }

        public void Persister(string type)
        {
            mapping.Set(x => x.Persister, Layer.Conventions, new TypeReference(type));
        }

        public void BatchSize(int batchSize)
        {
            mapping.Set(x => x.BatchSize, Layer.Conventions, batchSize);
        }
    }
}