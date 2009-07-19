using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Identity
{
    public class CompositeIdMapping : MappingBase, IIdentityMapping
    {
        private readonly AttributeStore<CompositeIdMapping> attributes = new AttributeStore<CompositeIdMapping>();
        private readonly IList<KeyPropertyMapping> keyProperties = new List<KeyPropertyMapping>();
        private readonly IList<KeyManyToOneMapping> keyManyToOnes = new List<KeyManyToOneMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCompositeId(this);

            foreach (var key in keyProperties)
                visitor.Visit(key);

            foreach (var key in keyManyToOnes)
                visitor.Visit(key);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string UnsavedValue
        {
            get { return attributes.Get(x => x.UnsavedValue); }
            set { attributes.Set(x => x.UnsavedValue, value); }
        }

        public IEnumerable<KeyPropertyMapping> KeyProperties
        {
            get { return keyProperties; }
        }

        public IEnumerable<KeyManyToOneMapping> KeyManyToOnes
        {
            get { return keyManyToOnes; }
        }

        public Type ContainingEntityType { get; set; }

        public void AddKeyProperty(KeyPropertyMapping mapping)
        {
            keyProperties.Add(mapping);
        }

        public void AddKeyManyToOne(KeyManyToOneMapping mapping)
        {
            keyManyToOnes.Add(mapping);
        }

        public bool IsSpecified<TResult>(Expression<Func<CompositeIdMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<CompositeIdMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<CompositeIdMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}