using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class CollectionInstance : ICollectionInstance
    {
        protected readonly InspectorModelMapper<ICollectionInspector, ICollectionMapping> propertyMappings = new InspectorModelMapper<ICollectionInspector, ICollectionMapping>();
        protected readonly ICollectionMapping mapping;
        protected bool nextBool;

        public CollectionInstance(ICollectionMapping mapping)
        {
            this.mapping = mapping;
            
            propertyMappings.Map(x => x.TableName, x => x.TableName);

            nextBool = true;
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }
        /// <summary>
        /// Represents a string identifier for the model instance, used in conventions for a lazy
        /// shortcut.
        /// 
        /// e.g. for a ColumnMapping the StringIdentifierForModel would be the Name attribute,
        /// this allows the user to find any columns with the matching name.
        /// </summary>
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }

        IKeyInspector ICollectionInspector.Key
        {
            get { return Key; }
        }
        public IRelationshipInstance Relationship
        {
            get { throw new NotImplementedException(); }
        }
        public void SetTableName(string tableName)
        {
            mapping.TableName = tableName;
        }

        public void Name(string name)
        {
            mapping.Name = name;
        }

        public void SchemaIs(string schema)
        {
            mapping.Schema = schema;
        }

        public void LazyLoad()
        {
            mapping.Lazy = nextBool ? Laziness.True : Laziness.False;
        }

        public ICollectionInstance Not
        {
            get 
            {
                nextBool = !nextBool;
                return this; 
            }
        }
        IKeyInstance ICollectionInstance.Key
        {
            get { return Key; }
        }
        public string TableName
        {
            get { return mapping.TableName; }
        }
        public bool IsMethodAccess
        {
            get { return Member != null && Member.MemberType == MemberTypes.Method; }
        }
        public MemberInfo Member
        {
            get { return mapping.MemberInfo; }
        }
        IRelationshipInspector ICollectionInspector.Relationship
        {
            get { return Relationship; }
        }
        public IAccessStrategyBuilder Access
        {
            get { return new AccessStrategyBuilder<CollectionInstance>(this, value => mapping.Access = value); }
        }

        public IKeyInstance Key
        {
            get { return new KeyInstance(mapping.Key); }
        }
    }
}