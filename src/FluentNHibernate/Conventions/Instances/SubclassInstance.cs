using System;
using System.Diagnostics;
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
            mapping.Set(x => x.DiscriminatorValue, Layer.Conventions, value);
        }

        public new void Abstract()
        {
            mapping.Set(x => x.Abstract, Layer.Conventions, nextBool);
            nextBool = true;
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

        public new void SelectBeforeUpdate()
        {
            mapping.Set(x => x.SelectBeforeUpdate, Layer.Conventions, nextBool);
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
            mapping.Set(x => x.Extends, Layer.Conventions, type);
        }
    }
}