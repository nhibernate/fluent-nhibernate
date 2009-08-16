using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Identity
{
    public class CompositeIdMapping : MappingBase, IIdentityMapping
    {
        private readonly AttributeStore<CompositeIdMapping> attributes;
        private readonly IList<KeyPropertyMapping> keyProperties = new List<KeyPropertyMapping>();
        private readonly IList<KeyManyToOneMapping> keyManyToOnes = new List<KeyManyToOneMapping>();

        public CompositeIdMapping()
            : this(new AttributeStore())
        {}

        public CompositeIdMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<CompositeIdMapping>(underlyingStore);
            attributes.SetDefault(x => x.Mapped, false);
            attributes.SetDefault(x => x.UnsavedValue, "undefined");
        }

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

        public bool Mapped
        {
            get { return attributes.Get(x => x.Mapped); }
            set { attributes.Set(x => x.Mapped, value); }
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