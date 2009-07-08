using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ManyToManyCollectionInspector : IManyToManyCollectionInspector
    {
        private readonly InspectorModelMapper<IManyToManyCollectionInspector, ICollectionMapping> propertyMappings = new InspectorModelMapper<IManyToManyCollectionInspector, ICollectionMapping>();
        private readonly ICollectionMapping mapping;

        public ManyToManyCollectionInspector(ICollectionMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.Map(x => x.TableName, x => x.TableName);
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
            return mapping.Attributes.IsSpecified(propertyMappings.Get(property));
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

        public IManyToManyInspector Relationship
        {
            get { return new ManyToManyInspector((ManyToManyMapping)mapping.Relationship); }
        }

        public Type ChildType
        {
            get { return mapping.ChildType; }
        }
        IRelationshipInspector ICollectionInspector.Relationship
        {
            get { return Relationship; }
        }
    }
}