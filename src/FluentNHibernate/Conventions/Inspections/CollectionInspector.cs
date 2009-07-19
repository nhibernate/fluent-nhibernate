using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CollectionInspector : ICollectionInspector
    {
        protected readonly InspectorModelMapper<ICollectionInspector, ICollectionMapping> propertyMappings = new InspectorModelMapper<ICollectionInspector, ICollectionMapping>();
        private readonly ICollectionMapping mapping;

        public CollectionInspector(ICollectionMapping mapping)
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
            get { return mapping.Name; }
        }

        /// <summary>
        /// Represents a string identifier for the model instance, used in conventions for a lazy
        /// shortcut.
        /// 
        /// e.g. for a ColumnMapping the StringIdentifierForModel would be the Name attribute,
        /// this allows the user to find any columns with the matching name.
        /// </summary>
        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        public IKeyInspector Key
        {
            get { throw new NotImplementedException(); }
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

        public IRelationshipInspector Relationship
        {
            get { return new RelationshipInspector(mapping.Relationship); }
        }

        public string Cascade
        {
            get { throw new NotImplementedException(); }
        }
        public string Fetch
        {
            get { throw new NotImplementedException(); }
        }
        public string OptimisticLock
        {
            get { throw new NotImplementedException(); }
        }
        public string OuterJoin
        {
            get { throw new NotImplementedException(); }
        }
        public bool Generic
        {
            get { throw new NotImplementedException(); }
        }
        public bool Inverse
        {
            get { throw new NotImplementedException(); }
        }
    }
}