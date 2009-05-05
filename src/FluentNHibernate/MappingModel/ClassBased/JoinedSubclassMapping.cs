using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class JoinedSubclassMapping : ClassMappingBase, ISubclassMapping
    {
        private readonly AttributeStore<JoinedSubclassMapping> attributes;
        private readonly IList<JoinedSubclassMapping> subclasses;
        private readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();
        public KeyMapping Key { get; set; }

        public JoinedSubclassMapping() : this(new AttributeStore())
        {}

        protected JoinedSubclassMapping(AttributeStore store) : base(store)
        {
            subclasses = new List<JoinedSubclassMapping>();
            attributes = new AttributeStore<JoinedSubclassMapping>(store);
        }

        public AttributeStore<JoinedSubclassMapping> Attributes
        {
            get { return attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoinedSubclass(this);

            if(Key != null)
                visitor.Visit(Key);

            foreach (var subclass in subclasses)
                visitor.Visit(subclass);

            base.AcceptVisitor(visitor);
        }

        public IEnumerable<JoinedSubclassMapping> Subclasses
        {
            get { return subclasses; }
        }

        public void AddSubclass(JoinedSubclassMapping joinedSubclassMapping)
        {
            subclasses.Add(joinedSubclassMapping);
        }

        public string TableName
        {
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }

        public Type Proxy
        {
            get { return attributes.Get(x => x.Proxy); }
            set { attributes.Set(x => x.Proxy, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public bool DynamicUpdate
        {
            get { return attributes.Get(x => x.DynamicUpdate); }
            set { attributes.Set(x => x.DynamicUpdate, value); }
        }

        public bool DynamicInsert
        {
            get { return attributes.Get(x => x.DynamicInsert); }
            set { attributes.Set(x => x.DynamicInsert, value); }
        }

        public bool SelectBeforeUpdate
        {
            get { return attributes.Get(x => x.SelectBeforeUpdate); }
            set { attributes.Set(x => x.SelectBeforeUpdate, value); }
        }

        public bool Abstract
        {
            get { return attributes.Get(x => x.Abstract); }
            set { attributes.Set(x => x.Abstract, value); }
        }

        public IEnumerable<IMappingPart> UnmigratedParts
        {
            get { return unmigratedParts; }
        }

        public IEnumerable<KeyValuePair<string, string>> UnmigratedAttributes
        {
            get { return unmigratedAttributes; }
        }

        public void AddUnmigratedPart(IMappingPart part)
        {
            unmigratedParts.Add(part);
        }

        public void AddUnmigratedAttribute(string attribute, string value)
        {
            unmigratedAttributes.Add(attribute, value);
        }
    }
}