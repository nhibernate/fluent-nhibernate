using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class VersionInspector : IVersionInspector
    {
        private readonly VersionMapping mapping;

        public VersionInspector(VersionMapping mapping)
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
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return mapping.Name; }
        }
    }
}