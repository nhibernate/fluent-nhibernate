using System;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class OneToManyCollectionInspector : IOneToManyCollectionInspector
    {
        private readonly ICollectionMapping mapping;

        public OneToManyCollectionInspector(ICollectionMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public IKeyInspector Key
        {
            get { return new KeyInspector(mapping.Key); }
        }
        public string TableName
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsMethodAccess
        {
            get { throw new NotImplementedException(); }
        }
        public MemberInfo Member
        {
            get { throw new NotImplementedException(); }
        }
        IOneToManyInspector IOneToManyCollectionInspector.Relationship
        {
            get { throw new NotImplementedException(); }
        }
        IRelationshipInspector ICollectionInspector.Relationship
        {
            get { throw new NotImplementedException(); }
        }
    }
}