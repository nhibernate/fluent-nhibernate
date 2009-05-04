using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : ClassMappingBase
    {
        private readonly AttributeStore<ClassMapping> _attributes;
        private readonly IList<ISubclassMapping> _subclasses;
        private DiscriminatorMapping discriminator;
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
            get { return discriminator; }
            set
            {
                if (discriminator != null)
                    discriminator.ParentClass = null;

                discriminator = value;

                if (discriminator != null)
                    discriminator.ParentClass = this;
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

        public string TableName
        {
            get { return _attributes.Get(x => x.TableName); }
            set { _attributes.Set(x => x.TableName, value); }
        }

        public int BatchSize
        {
            get { return _attributes.Get(x => x.BatchSize); }
            set { _attributes.Set(x => x.BatchSize, value); }
        }

        public object DiscriminatorBaseValue
        {
            get { return _attributes.Get(x => x.DiscriminatorBaseValue); }
            set { _attributes.Set(x => x.DiscriminatorBaseValue, value); }
        }

        public string Schema
        {
            get { return _attributes.Get(x => x.Schema); }
            set { _attributes.Set(x => x.Schema, value); }
        }

        public bool LazyLoad
        {
            get { return _attributes.Get(x => x.LazyLoad); }
            set { _attributes.Set(x => x.LazyLoad, value); }
        }

        public bool Mutable
        {
            get { return _attributes.Get(x => x.Mutable); }
            set { _attributes.Set(x => x.Mutable, value); }
        }

        public bool DynamicUpdate
        {
            get { return _attributes.Get(x => x.DynamicUpdate); }
            set { _attributes.Set(x => x.DynamicUpdate, value); }
        }

        public bool DynamicInsert
        {
            get { return _attributes.Get(x => x.DynamicInsert); }
            set { _attributes.Set(x => x.DynamicInsert, value); }
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