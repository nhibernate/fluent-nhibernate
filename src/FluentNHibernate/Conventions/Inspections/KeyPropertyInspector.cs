using System;
using System.Reflection;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public class KeyPropertyInspector : IKeyPropertyInspector
    {
        private readonly KeyPropertyMapping mapping;

        public KeyPropertyInspector(KeyPropertyMapping mapping)
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
    }
}