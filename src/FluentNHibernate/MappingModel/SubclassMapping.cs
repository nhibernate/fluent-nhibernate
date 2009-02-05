using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MappingModel
{
    public class SubclassMapping : ClassMappingBase, ISubclassMapping
    {
        private AttributeStore<SubclassMapping> _attributes;
        private IList<SubclassMapping> _subclasses;

        public SubclassMapping()
            : this(new AttributeStore())
        { }

        protected SubclassMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            _attributes = new AttributeStore<SubclassMapping>(underlyingStore);
            _subclasses = new List<SubclassMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessSubclass(this);

            foreach(var subclass in Subclasses)
                visitor.Visit(subclass);

            base.AcceptVisitor(visitor);
        }

        public void AddSubclass(SubclassMapping subclassMapping)
        {
            _subclasses.Add(subclassMapping);
        }

        public IEnumerable<SubclassMapping> Subclasses
        {
            get { return _subclasses; }
        }
    }
}
