using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel
{
    public class SubclassMapping : ClassMappingBase, ISubclassMapping
    {
        private readonly AttributeStore<SubclassMapping> _attributes;
        private readonly IList<SubclassMapping> _subclasses;
        private readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public SubclassMapping()
            : this(new AttributeStore())
        { }

        protected SubclassMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            _attributes = new AttributeStore<SubclassMapping>(underlyingStore);
            _subclasses = new List<SubclassMapping>();
        }

        public AttributeStore<SubclassMapping> Attributes
        {
            get { return _attributes; }
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

        public object DiscriminatorValue
        {
            get { return _attributes.Get(x => x.DiscriminatorValue); }
            set { _attributes.Set(x => x.DiscriminatorValue, value); }
        }

        public bool LazyLoad
        {
            get { return _attributes.Get(x => x.LazyLoad); }
            set { _attributes.Set(x => x.LazyLoad, value); }
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
