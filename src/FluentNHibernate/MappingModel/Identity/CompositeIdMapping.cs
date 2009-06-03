using System;
using System.Collections.Generic;

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

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string UnsavedValue
        {
            get { return attributes.Get(x => x.UnsavedValue); }
            set { attributes.Set(x => x.UnsavedValue, value); }
        }

        public AttributeStore<CompositeIdMapping> Attributes
        {
            get { return attributes; }
        }

        public IEnumerable<KeyPropertyMapping> KeyProperties
        {
            get { return keyProperties; }
        }

        public IEnumerable<KeyManyToOneMapping> KeyManyToOnes
        {
            get { return keyManyToOnes; }
        }

        public void AddKeyProperty(KeyPropertyMapping mapping)
        {
            keyProperties.Add(mapping);
        }

        public void AddKeyManyToOne(KeyManyToOneMapping mapping)
        {
            keyManyToOnes.Add(mapping);
        }
    }
}