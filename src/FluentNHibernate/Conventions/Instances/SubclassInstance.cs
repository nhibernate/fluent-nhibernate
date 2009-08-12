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

        public new void DiscriminatorValue(object value)
        {
            if (!mapping.IsSpecified(x => x.DiscriminatorValue))
                mapping.DiscriminatorValue = value;
        }

        public new void Abstract()
        {
            if (mapping.IsSpecified(x => x.Abstract))
                return;

            mapping.Abstract = nextBool;
            nextBool = true;
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

        public new void SelectBeforeUpdate()
        {
            if (mapping.IsSpecified(x => x.SelectBeforeUpdate))
                return;

            mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
        }
    }
}