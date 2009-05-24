using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class PropertyMapping : MappingBase, INameable
    {
        private readonly List<ColumnMapping> columns = new List<ColumnMapping>();
        private readonly AttributeStore<PropertyMapping> attributes = new AttributeStore<PropertyMapping>();
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public PropertyMapping(Type containingEntityType)
        {
            ContainingEntityType = containingEntityType;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessProperty(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public AttributeStore<PropertyMapping> Attributes
        {
            get { return attributes; }
        }

        public IDictionary<string, string> UnmigratedAttributes
        {
            get { return unmigratedAttributes; }
        }

        public Type ContainingEntityType
        {
            get { return attributes.Get(x => x.ContainingEntityType); }
            set { attributes.Set(x => x.ContainingEntityType, value); }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public bool IsNameSpecified
        {
            get { return attributes.IsSpecified(x => x.Name); }
        }

        public bool Insert
        {
            get { return attributes.Get(x => x.Insert); }
            set { attributes.Set(x => x.Insert, value); }
        }

        public bool Update
        {
            get { return attributes.Get(x => x.Update); }
            set { attributes.Set(x => x.Update, value); }
        }

        public string Formula
        {
            get { return attributes.Get(x => x.Formula); }
            set { attributes.Set(x => x.Formula, value); }
        }

        public string Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public string UniqueKey
        {
            get { return attributes.Get(x => x.UniqueKey); }
            set { attributes.Set(x => x.UniqueKey, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public PropertyInfo PropertyInfo { get; set; }
        
        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping mapping)
        {
            columns.Add(mapping);
        }

        public void AddUnmigratedAttribute(string name, string value)
        {
            unmigratedAttributes[name] = value;
        }
    }
}