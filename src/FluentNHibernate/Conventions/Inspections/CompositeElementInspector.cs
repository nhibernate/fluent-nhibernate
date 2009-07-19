using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CompositeElementInspector : ICompositeElementInspector
    {
        private readonly CompositeElementMapping mapping;

        public CompositeElementInspector(CompositeElementMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }
    }
}