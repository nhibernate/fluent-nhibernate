using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class NaturalIdMapping : MappingBase
    {
        private readonly AttributeStore<NaturalIdMapping> attributes;
        private readonly IList<PropertyMapping> properties = new List<PropertyMapping>();
        private readonly IList<ManyToOneMapping> manyToOnes = new List<ManyToOneMapping>();

        public NaturalIdMapping()
            : this(new AttributeStore()) { }

        public NaturalIdMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<NaturalIdMapping>(underlyingStore);
            attributes.SetDefault(x => x.Mutable, false);
        }

        public bool Mutable
        {
            get { return attributes.Get(x => x.Mutable); }
            set { attributes.Set(x => x.Mutable, value); }
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return properties; }
        }

        public IEnumerable<ManyToOneMapping> ManyToOnes
        {
            get { return manyToOnes; }
        }

        public void AddProperty(PropertyMapping mapping)
        {
            properties.Add(mapping);
        }

        public void AddReference(ManyToOneMapping mapping)
        {
            manyToOnes.Add(mapping);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessNaturalId(this);

            foreach (var key in properties)
                visitor.Visit(key);

            foreach (var key in manyToOnes)
                visitor.Visit(key);
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<NaturalIdMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<NaturalIdMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}
