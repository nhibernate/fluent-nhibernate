using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class OneToOneInspector : IOneToOneInspector
    {
        private readonly OneToOneMapping mapping;

        public OneToOneInspector(OneToOneMapping mapping)
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
    }
}