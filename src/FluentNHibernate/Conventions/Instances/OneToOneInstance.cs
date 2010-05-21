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
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }

        public new ICascadeInstance Cascade
        {
            get
            {
                return new CascadeInstance(value =>
                {
                    if (!mapping.IsSpecified("Cascade"))
                        mapping.Cascade = value;
                });
            }
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
            get
            {
                return new FetchInstance(value =>
                {
                    if (!mapping.IsSpecified("Fetch"))
                        mapping.Fetch = value;
                });
            }
        }

        public new void Class<T>()
        {
            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(typeof(T));
        }

        public new void Class(Type type)
        {
            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(type);
        }

        public new void Constrained()
        {
            if (!mapping.IsSpecified("Constrained"))
                mapping.Constrained = nextBool;
            nextBool = true;
        }

        public new void ForeignKey(string key)
        {
            if (!mapping.IsSpecified("ForeignKey"))
                mapping.ForeignKey = key;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
            {
                if (nextBool)
                    LazyLoad(Laziness.Proxy);
                else
                    LazyLoad(Laziness.False);
            }
            nextBool = true;
        }

        public new void LazyLoad(Laziness laziness)
        {
            mapping.Lazy = laziness.ToString();
            nextBool = true;
        }

        public new void PropertyRef(string propertyName)
        {
            if (!mapping.IsSpecified("PropertyRef"))
                mapping.PropertyRef = propertyName;
        }

        public void OverrideInferredClass(Type type)
        {
            mapping.Class = new TypeReference(type);
        }
    }
}