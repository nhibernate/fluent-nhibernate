using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel
{
    public class JoinMapping : IMappingBase
    {
        private readonly AttributeStore<JoinMapping> attributes;

        public KeyMapping Key { get; set; }
        private readonly MappedMembers mappedMembers;

        private readonly List<IMappingPart> unmigratedParts = new List<IMappingPart>();
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public JoinMapping()
        {
            attributes = new AttributeStore<JoinMapping>();
            mappedMembers = new MappedMembers();
        }

        public IEnumerable<PropertyMapping> Properties
        {
            get { return mappedMembers.Properties; }
        }

        public IEnumerable<ManyToOneMapping> References
        {
            get { return mappedMembers.References; }
        }

        public IEnumerable<ComponentMappingBase> Components
        {
            get { return mappedMembers.Components; }
        }

        public void AddProperty(PropertyMapping property)
        {
            mappedMembers.AddProperty(property);
        }

        public void AddReference(ManyToOneMapping manyToOne)
        {
            mappedMembers.AddReference(manyToOne);
        }

        public void AddComponent(ComponentMapping componentMapping)
        {
            mappedMembers.AddComponent(componentMapping);
        }

        public string TableName
        {
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public virtual void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoin(this);

            if (Key != null)
                visitor.Visit(Key);

            mappedMembers.AcceptVisitor(visitor);
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
