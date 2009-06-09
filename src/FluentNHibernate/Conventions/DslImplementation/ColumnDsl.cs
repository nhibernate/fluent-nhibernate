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
        private readonly InspectorModelMapper<IColumnInspector, ColumnMapping> propertyMappings = new InspectorModelMapper<IColumnInspector, ColumnMapping>();

        public ColumnDsl(PropertyMapping propertyMapping, ColumnMapping mapping)
        {
            this.propertyMapping = propertyMapping;
            this.mapping = mapping;

            propertyMappings.Map(x => x.Check, x => x.Check);
            propertyMappings.Map(x => x.Index, x => x.Index);
            propertyMappings.Map(x => x.Length, x => x.Length);
            propertyMappings.Map(x => x.Name, x => x.Name);
            propertyMappings.Map(x => x.NotNull, x => x.NotNull);
            propertyMappings.Map(x => x.SqlType, x => x.SqlType);
            propertyMappings.Map(x => x.Unique, x => x.Unique);
            propertyMappings.Map(x => x.UniqueKey, x => x.UniqueKey);
        }

        public Type EntityType
        {
            get { return propertyMapping.ContainingEntityType; }
        }

        string IColumnInspector.Name
        {
            get { return mapping.Name; }
        }

        string IColumnInspector.Check
        {
            get { return mapping.Check; }
        }

        string IColumnInspector.Index
        {
            get { return mapping.Index; }
        }

        int IColumnInspector.Length
        {
            get { return mapping.Length; }
        }

        bool IColumnInspector.NotNull
        {
            get { return mapping.NotNull; }
        }

        string IColumnInspector.SqlType
        {
            get { return mapping.SqlType; }
        }

        bool IColumnInspector.Unique
        {
            get { return mapping.Unique; }
        }

        string IColumnInspector.UniqueKey
        {
            get { return mapping.UniqueKey; }
        }

        string IInspector.StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        bool IInspector.IsSet(PropertyInfo property)
        {
            return mapping.Attributes.IsSpecified(propertyMappings.Get(property));
        }
    }
}