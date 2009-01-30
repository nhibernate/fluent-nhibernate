using System;

namespace FluentNHibernate.MappingModel
{
    public class JoinedSubclassMapping : MappingBase, ISubclassMapping
    {
        private AttributeStore<JoinedSubclassMapping> _attributes;

        public JoinedSubclassMapping()
        {
            _attributes = new AttributeStore<JoinedSubclassMapping>();
        }

        public AttributeStore<JoinedSubclassMapping> Attributes
        {
            get { return _attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoinedSubclass(this);
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        
    }
}