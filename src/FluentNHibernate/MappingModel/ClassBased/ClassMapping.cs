using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.ClassBased
{
    [Serializable]
    public class ClassMapping : ClassMappingBase
    {
        readonly AttributeStore attributes;

        public ClassMapping()
            : this(new AttributeStore())
        {}

        public ClassMapping(AttributeStore attributes)
            : base(attributes)
        {
            this.attributes = attributes;
        }

        public IIdentityMapping Id
        {
            get { return attributes.GetOrDefault<IIdentityMapping>("Id"); }
        }

        public NaturalIdMapping NaturalId
        {
            get { return attributes.GetOrDefault<NaturalIdMapping>("NaturalId"); }
        }

        public override string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public override Type Type
        {
            get { return attributes.GetOrDefault<Type>("Type"); }
        }

        public CacheMapping Cache
        {
            get { return attributes.GetOrDefault<CacheMapping>("Cache"); }
        }

        public VersionMapping Version
        {
            get { return attributes.GetOrDefault<VersionMapping>("Version"); }
        }

        public DiscriminatorMapping Discriminator
        {
            get { return attributes.GetOrDefault<DiscriminatorMapping>("Discriminator"); }
        }

        public bool IsUnionSubclass
        {
            get { return attributes.GetOrDefault<bool>("IsUnionSubclass"); }
        }

        public TuplizerMapping Tuplizer
        {
            get { return attributes.GetOrDefault<TuplizerMapping>("Tuplizer"); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessClass(this);            

            if (Id != null)
                visitor.Visit(Id);

            if (NaturalId != null)
                visitor.Visit(NaturalId);

            if (Discriminator != null)
                visitor.Visit(Discriminator);

            if (Cache != null)
                visitor.Visit(Cache);

            if (Version != null)
                visitor.Visit(Version);

            if (Tuplizer != null)
                visitor.Visit(Tuplizer);

            base.AcceptVisitor(visitor);
        }

        public string TableName
        {
            get { return attributes.GetOrDefault<string>("TableName"); }
        }

        public int BatchSize
        {
            get { return attributes.GetOrDefault<int>("BatchSize"); }
        }

        public object DiscriminatorValue
        {
            get { return attributes.GetOrDefault<object>("DiscriminatorValue"); }
        }

        public string Schema
        {
            get { return attributes.GetOrDefault<string>("Schema"); }
        }

        public bool Lazy
        {
            get { return attributes.GetOrDefault<bool>("Lazy"); }
        }

        public bool Mutable
        {
            get { return attributes.GetOrDefault<bool>("Mutable"); }
        }

        public bool DynamicUpdate
        {
            get { return attributes.GetOrDefault<bool>("DynamicUpdate"); }
        }

        public bool DynamicInsert
        {
            get { return attributes.GetOrDefault<bool>("DynamicInsert"); }
        }

        public string OptimisticLock
        {
            get { return attributes.GetOrDefault<string>("OptimisticLock"); }
        }

        public string Polymorphism
        {
            get { return attributes.GetOrDefault<string>("Polymorphism"); }
        }

        public string Persister
        {
            get { return attributes.GetOrDefault<string>("Persister"); }
        }

        public string Where
        {
            get { return attributes.GetOrDefault<string>("Where"); }
        }

        public string Check
        {
            get { return attributes.GetOrDefault<string>("Check"); }
        }

        public string Proxy
        {
            get { return attributes.GetOrDefault<string>("Proxy"); }
        }

        public bool SelectBeforeUpdate
        {
            get { return attributes.GetOrDefault<bool>("SelectBeforeUpdate"); }
        }

        public bool Abstract
        {
            get { return attributes.GetOrDefault<bool>("Abstract"); }
        }

        public string Subselect
        {
            get { return attributes.GetOrDefault<string>("Subselect"); }
        }

        public string SchemaAction
        {
            get { return attributes.GetOrDefault<string>("SchemaAction"); }
        }

        public string EntityName
        {
            get { return attributes.GetOrDefault<string>("EntityName"); }
        }       

        public bool Equals(ClassMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) &&
                Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ClassMapping)) return false;
            return Equals((ClassMapping)obj);
        }

        public override int GetHashCode()
        {
            return (attributes != null ? attributes.GetHashCode() : 0);
        }

        public void Set<T>(Expression<Func<ClassMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}