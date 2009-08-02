using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public class IdentityInspector : IIdentityInspector
    {
        private readonly InspectorModelMapper<IIdentityInspector, IdMapping> propertyMappings = new InspectorModelMapper<IIdentityInspector, IdMapping>();
        private readonly IdMapping mapping;

        public IdentityInspector(IdMapping mapping)
        {
            this.mapping = mapping;
            propertyMappings.AutoMap();
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                return mapping.Columns.UserDefined
                    .Select(x => new ColumnInspector(EntityType, x))
                    .Cast<IColumnInspector>();
            }
        }

        public IGeneratorInspector Generator
        {
            get
            {
                if (mapping.Generator == null)
                    return new GeneratorInspector(new GeneratorMapping());

                return new GeneratorInspector(mapping.Generator);
            }
        }

        public string UnsavedValue
        {
            get { return mapping.UnsavedValue; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public TypeReference Type
        {
            get { return mapping.Type; }
        }

        public int Length
        {
            get { return mapping.Length; }
        }
    }
}