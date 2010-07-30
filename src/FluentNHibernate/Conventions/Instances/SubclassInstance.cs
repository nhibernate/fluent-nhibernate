using System;
using System.Diagnostics;
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            if (!mapping.IsSpecified("DiscriminatorValue"))
                mapping.DiscriminatorValue = value;
        }

        public new void Abstract()
        {
            if (!mapping.IsSpecified("Abstract"))
                mapping.Abstract = nextBool;
            nextBool = true;
        }

        public new void DynamicInsert()
        {
            if (!mapping.IsSpecified("DynamicInsert"))
                mapping.DynamicInsert = nextBool;
            nextBool = true;
        }

        public new void DynamicUpdate()
        {
            if (!mapping.IsSpecified("DynamicUpdate"))
                mapping.DynamicUpdate = nextBool;
            nextBool = true;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
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

        public new void SelectBeforeUpdate()
        {
            if (!mapping.IsSpecified("SelectBeforeUpdate"))
                mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// (optional) Specifies the entity from which this subclass descends/extends.
        /// </summary>
        /// <typeparam name="T">Type of the entity to extend</typeparam>
        public new void Extends<T>()
        {
            Extends(typeof(T));
        }

        /// <summary>
        /// (optional) Specifies the entity from which this subclass descends/extends.
        /// </summary>
        /// <param name="type">Type of the entity to extend</param>
        public new void Extends(Type type)
        {
            if (!mapping.IsSpecified("Extends"))
                mapping.Extends = type;
        }
    }
}