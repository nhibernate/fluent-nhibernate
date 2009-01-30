using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public class JoinedSubclassMapping : MappingBase, ISubclassMapping
    {
        private IList<JoinedSubclassMapping> _subclasses;
        private AttributeStore<JoinedSubclassMapping> _attributes;

        public JoinedSubclassMapping()
        {
            _subclasses = new List<JoinedSubclassMapping>();
            _attributes = new AttributeStore<JoinedSubclassMapping>();
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

            foreach(var subclass in _subclasses)
                visitor.Visit(subclass);
        }

        public KeyMapping Key { get; set; }

        public IEnumerable<JoinedSubclassMapping> Subclasses
        {
            get { return _subclasses; }
        }

        public void AddSubclass(JoinedSubclassMapping joinedSubclassMapping)
        {
            _subclasses.Add(joinedSubclassMapping);
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }
    }
}