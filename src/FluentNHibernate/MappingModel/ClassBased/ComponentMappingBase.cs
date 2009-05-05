using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public abstract class ComponentMappingBase<T> : ClassMappingBase where T : ComponentMappingBase<T> 
    {
        protected readonly AttributeStore<T> attributes;
        protected readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        protected readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        protected ComponentMappingBase()
            : this(new AttributeStore())
        {}

        protected ComponentMappingBase(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<T>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            foreach (var property in Properties)
                visitor.Visit(property);

            if (Parent != null)
                visitor.Visit(Parent);

            base.AcceptVisitor(visitor);
        }

        public ParentMapping Parent { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public string PropertyName
        {
            get { return attributes.Get(x => x.PropertyName); }
            set { attributes.Set(x => x.PropertyName, value); }
        }

        public AttributeStore<T> Attributes
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