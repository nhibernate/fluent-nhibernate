using System;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
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

        public IAccessInstance Access
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

        public ICascadeInstance Cascade
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

        public IFetchInstance Fetch
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

        public void Class<T>()
        {
            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(typeof(T));
        }

        public void Class(Type type)
        {
            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(type);
        }

        public void Constrained()
        {
            if (mapping.IsSpecified("Constrained"))
                return;

            mapping.Constrained = nextBool;
            nextBool = true;
        }

        public void ForeignKey(string key)
        {
            if (!mapping.IsSpecified("ForeignKey"))
                mapping.ForeignKey = key;
        }

        public void LazyLoad()
        {
            if (mapping.IsSpecified("Lazy"))
                return;

            mapping.Lazy = nextBool;
            nextBool = true;
        }

        public void PropertyRef(string propertyName)
        {
            if (!mapping.IsSpecified("PropertyRef"))
                mapping.PropertyRef = propertyName;
        }
    }
}