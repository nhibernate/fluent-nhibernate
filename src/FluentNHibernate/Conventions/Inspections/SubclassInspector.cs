using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class SubclassInspector : ISubclassInspector
    {
        private readonly SubclassMapping mapping;

        public SubclassInspector(SubclassMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { return mapping.Type; }
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