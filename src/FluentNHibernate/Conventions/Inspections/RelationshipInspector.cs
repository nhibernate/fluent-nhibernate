using System;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class RelationshipInspector : IRelationshipInspector
    {
        private readonly ICollectionRelationshipMapping mapping;

        public RelationshipInspector(ICollectionRelationshipMapping mapping)
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