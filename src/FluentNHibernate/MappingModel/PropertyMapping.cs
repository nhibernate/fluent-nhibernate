using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class PropertyMapping : MappingBase, INameable
    {
        private readonly List<ColumnMapping> columns = new List<ColumnMapping>();
        private readonly AttributeStore<PropertyMapping> attributes = new AttributeStore<PropertyMapping>();

        public PropertyMapping()
        {}

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

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
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

        public bool OptimisticLock
        {
            get { return attributes.Get(x => x.OptimisticLock); }
            set { attributes.Set(x => x.OptimisticLock, value); }
        }

        public string Generated
        {
            get { return attributes.Get(x => x.Generated); }
            set { attributes.Set(x => x.Generated, value); }
        }

        public string Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
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
    }
}