using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : ClassMappingBase
    {
        private readonly AttributeStore<ClassMapping> _attributes;
        private readonly IList<ISubclassMapping> _subclasses;
        private DiscriminatorMapping _discriminator;
        private readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();
        public IIdentityMapping Id { get; set; }

        public ClassMapping()
            : this(new AttributeStore())
        {}

        public ClassMapping(Type type)
            : this(new AttributeStore())
        {
            Type = type;
        }

        protected ClassMapping(AttributeStore store)
            : base(store)
        {
            _attributes = new AttributeStore<ClassMapping>(store);
            _subclasses = new List<ISubclassMapping>();
        }

        public DiscriminatorMapping Discriminator
        {
            get { return _discriminator; }
            set
            {
                if (_discriminator != null)
                    _discriminator.ParentClass = null;

                _discriminator = value;

                if (_discriminator != null)
                    _discriminator.ParentClass = this;
            }
        }

        public IEnumerable<ISubclassMapping> Subclasses
        {
            get { return _subclasses; }
        }

        public void AddSubclass(ISubclassMapping subclass)
        {
            _subclasses.Add(subclass);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessClass(this);            

            if (Id != null)
                visitor.Visit(Id);

            if (Discriminator != null)
                visitor.Visit(Discriminator);

            foreach (var subclass in Subclasses)
                visitor.Visit(subclass);

            base.AcceptVisitor(visitor);
        }

        public string Tablename
        {
            get { return _attributes.Get(x => x.Tablename); }
            set { _attributes.Set(x => x.Tablename, value); }
        }

        public int BatchSize
        {
            get { return _attributes.Get(x => x.BatchSize); }
            set { _attributes.Set(x => x.BatchSize, value); }
        }

        public AttributeStore<ClassMapping> Attributes
        {
            get { return _attributes; }
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