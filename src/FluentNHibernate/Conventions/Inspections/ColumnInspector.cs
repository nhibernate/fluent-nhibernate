using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ColumnInspector : IColumnInspector
    {
        private readonly ColumnMapping mapping;
        private readonly InspectorModelMapper<IColumnInspector, ColumnMapping> propertyMappings = new InspectorModelMapper<IColumnInspector, ColumnMapping>();

        public ColumnInspector(Type containingEntityType, ColumnMapping mapping)
        {
            EntityType = containingEntityType;
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

        public Type EntityType { get; private set; }

        public string Name
        {
            get { return mapping.Name; }
        }

        public string Check
        {
            get { return mapping.Check; }
        }

        public string Index
        {
            get { return mapping.Index; }
        }

        public int Length
        {
            get { return mapping.Length; }
        }

        public bool NotNull
        {
            get { return mapping.NotNull; }
        }

        public string SqlType
        {
            get { return mapping.SqlType; }
        }

        public bool Unique
        {
            get { return mapping.Unique; }
        }

        public string UniqueKey
        {
            get { return mapping.UniqueKey; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}