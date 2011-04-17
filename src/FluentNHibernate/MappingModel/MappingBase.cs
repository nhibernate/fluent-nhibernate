using System;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public abstract class MappingBase : IMapping
    {
        public abstract void AcceptVisitor(IMappingModelVisitor visitor);
        public abstract bool IsSpecified(string attribute);

        protected abstract void Set(string attribute, int layer, object value);

        void IMapping.Set(string attribute, int layer, object value)
        {
            Set(attribute, layer, value);
        }
    }
}