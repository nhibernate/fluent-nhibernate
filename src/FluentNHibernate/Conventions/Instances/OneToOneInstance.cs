using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class OneToOneInstance : OneToOneInspector, IOneToOneInstance
    {
        private readonly OneToOneMapping mapping;
        private bool nextBool = true;

        public OneToOneInstance(OneToOneMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new ICascadeInstance Cascade
        {
            get { return new CascadeInstance(value => mapping.Set(x => x.Cascade, Layer.Conventions, value)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IOneToOneInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new IFetchInstance Fetch
        {
            get { return new FetchInstance(value => mapping.Set(x => x.Fetch, Layer.Conventions, value)); }
        }

        public new void Class<T>()
        {
            Class(typeof(T));
        }

        public new void Class(Type type)
        {
            mapping.Set(x => x.Class, Layer.Conventions, new TypeReference(type));
        }

        public new void Constrained()
        {
            mapping.Set(x => x.Constrained, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void ForeignKey(string key)
        {
            mapping.Set(x => x.ForeignKey, Layer.Conventions, key);
        }

        public new void LazyLoad()
        {
            if (nextBool)
                LazyLoad(Laziness.Proxy);
            else
                LazyLoad(Laziness.False);
            nextBool = true;
        }

        public new void LazyLoad(Laziness laziness)
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, laziness.ToString());
            nextBool = true;
        }

        public new void PropertyRef(string propertyName)
        {
            mapping.Set(x => x.PropertyRef, Layer.Conventions, propertyName);
        }

        public void OverrideInferredClass(Type type)
        {
            mapping.Set(x => x.Class, Layer.Conventions, new TypeReference(type));
        }
    }
}