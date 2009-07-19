using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                foreach (var column in mapping.Columns)
                    yield return new ColumnInspector(EntityType, column);
            }
        }

        public Generator Generator
        {
            get { throw new NotImplementedException(); }
        }
        public object UnsavedValue
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return mapping.Name; }
        }
    }
}