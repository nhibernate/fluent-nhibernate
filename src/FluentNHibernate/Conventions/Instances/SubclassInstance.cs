using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Instances
{
    public class SubclassInstance : SubclassInspector, ISubclassInstance
    {
        private readonly SubclassMapping mapping;
        private bool nextBool = true;

        public SubclassInstance(SubclassMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public ISubclassInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public void Abstract()
        {
            if (mapping.IsSpecified(x => x.Abstract))
                return;

            mapping.Abstract = nextBool;
            nextBool = true;
        }

        public void DynamicInsert()
        {
            if (mapping.IsSpecified(x => x.DynamicInsert))
                return;

            mapping.DynamicInsert = nextBool;
            nextBool = true;
        }

        public void DynamicUpdate()
        {
            if (mapping.IsSpecified(x => x.DynamicUpdate))
                return;

            mapping.DynamicUpdate = nextBool;
            nextBool = true;
        }

        public void LazyLoad()
        {
            if (mapping.IsSpecified(x => x.Lazy))
                return;

            mapping.Lazy = nextBool ? Laziness.True : Laziness.False;
            nextBool = true;
        }

        public void Proxy(Type type)
        {
            if (!mapping.IsSpecified(x => x.Proxy))
                mapping.Proxy = type.AssemblyQualifiedName;
        }

        public void Proxy<T>()
        {
            if (!mapping.IsSpecified(x => x.Proxy))
                mapping.Proxy = typeof(T).AssemblyQualifiedName;
        }

        public void SelectBeforeUpdate()
        {
            if (mapping.IsSpecified(x => x.SelectBeforeUpdate))
                return;

            mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
        }
    }
}