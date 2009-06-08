using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class ColumnDsl : IColumnInspector
    {
        private readonly PropertyMapping propertyMapping;
        private readonly ColumnMapping mapping;

        public ColumnDsl(PropertyMapping propertyMapping, ColumnMapping mapping)
        {
            this.propertyMapping = propertyMapping;
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { return propertyMapping.ContainingEntityType; }
        }

        string IColumnInspector.Name
        {
            get { return mapping.Name; }
        }

        string IInspector.StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }
    }
}