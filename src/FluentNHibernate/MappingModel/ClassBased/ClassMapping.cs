using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ClassMapping : ClassMappingBase
    {
        private readonly AttributeStore<ClassMapping> attributes;
        private readonly IList<ISubclassMapping> subclasses;
        private readonly IList<JoinMapping> joins;
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
            attributes = new AttributeStore<ClassMapping>(store);
            subclasses = new List<ISubclassMapping>();
            joins = new List<JoinMapping>();
        }

        public CacheMapping Cache { get; set; }

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
            get { return subclasses; }
        }

        public IEnumerable<JoinMapping> Joins
        {
            get { return joins; }
        }

        public void AddSubclass(ISubclassMapping subclass)
        {
            subclasses.Add(subclass);
        }

        public void AddJoin(JoinMapping join)
        {
            joins.Add(join);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessClass(this);            

            if (Id != null)
                visitor.Visit(Id);

            if (Discriminator != null)
                visitor.Visit(Discriminator);

            if (Cache != null)
                visitor.Visit(Cache);

            foreach (var subclass in Subclasses)
                visitor.Visit(subclass);

            foreach (var join in Joins)
                visitor.Visit(join);

            base.AcceptVisitor(visitor);
        }

        public string TableName
        {
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public int BatchSize
        {
            get { return attributes.Get(x => x.BatchSize); }
            set { attributes.Set(x => x.BatchSize, value); }
        }

        public object DiscriminatorBaseValue
        {
            get { return attributes.Get(x => x.DiscriminatorBaseValue); }
            set { attributes.Set(x => x.DiscriminatorBaseValue, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public bool LazyLoad
        {
            get { return attributes.Get(x => x.LazyLoad); }
            set { attributes.Set(x => x.LazyLoad, value); }
        }

        public bool Mutable
        {
            get { return attributes.Get(x => x.Mutable); }
            set { attributes.Set(x => x.Mutable, value); }
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

        public string OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public AttributeStore<ClassMapping> Attributes
        {
            get { return attributes; }
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