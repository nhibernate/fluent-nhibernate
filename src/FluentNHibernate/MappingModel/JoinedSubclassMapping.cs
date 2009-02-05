using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    public class JoinedSubclassMapping : ClassMappingBase, ISubclassMapping
    {
        private readonly AttributeStore<JoinedSubclassMapping> _attributes;
        private readonly IList<JoinedSubclassMapping> _subclasses;
        public KeyMapping Key { get; set; }

        public JoinedSubclassMapping() : this(new AttributeStore())
        {
            
        }

        protected JoinedSubclassMapping(AttributeStore store) : base(store)
        {
            _subclasses = new List<JoinedSubclassMapping>();
            _attributes = new AttributeStore<JoinedSubclassMapping>(store);
        }

        public AttributeStore<JoinedSubclassMapping> Attributes
        {
            get { return _attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoinedSubclass(this);

            if(Key != null)
                visitor.Visit(Key);

            foreach (var subclass in _subclasses)
                visitor.Visit(subclass);

            base.AcceptVisitor(visitor);
        }

        public IEnumerable<JoinedSubclassMapping> Subclasses
        {
            get { return _subclasses; }
        }

        public void AddSubclass(JoinedSubclassMapping joinedSubclassMapping)
        {
            _subclasses.Add(joinedSubclassMapping);
        }
        
    }
}