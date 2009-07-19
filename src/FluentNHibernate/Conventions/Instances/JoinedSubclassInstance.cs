using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

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

        public void CheckConstraint(string constraint)
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

            mapping.Lazy = nextBool ? Laziness.True : Laziness.False;
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
    }
}