using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ClassMapping : ClassMappingBase
    {
        private readonly AttributeStore<ClassMapping> _attributes;
        private readonly IList<ISubclassMapping> _subclasses;
        public IIdentityMapping Id { get; set; }
        public DiscriminatorMapping Discriminator { get; set; }

        public ClassMapping()
            : this(new AttributeStore())
        { }

        protected ClassMapping(AttributeStore store)
            : base(store)
        {
            _attributes = new AttributeStore<ClassMapping>(store);
            _subclasses = new List<ISubclassMapping>();
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

        public AttributeStore<ClassMapping> Attributes
        {
            get { return _attributes; }
        }

        
    }
}