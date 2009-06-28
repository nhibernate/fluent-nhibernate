using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class CollectionInspector : ICollectionInspector
    {
        private readonly InspectorModelMapper<ICollectionInspector, ICollectionMapping> propertyMappings = new InspectorModelMapper<ICollectionInspector, ICollectionMapping>();
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
            get { return mapping.TableName; }
        }

        public bool IsMethodAccess
        {
            get { return mapping.MemberInfo is MethodInfo; }
        }

        public MemberInfo Member
        {
            get { return mapping.MemberInfo; }
        }

        public IManyToManyInspector ManyToMany
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new ManyToManyInspector((ManyToManyMapping)mapping.Relationship);

                // dummy inspector, won't actually contain anything
                return new ManyToManyInspector(new ManyToManyMapping());
            }
        }

        public Type ChildType
        {
            get { return mapping.ChildType; }
        }

        public IOneToManyInspector OneToMany
        {
            get
            {
                if (mapping.Relationship is ManyToManyMapping)
                    return new OneToManyInspector((OneToManyMapping)mapping.Relationship);

                // dummy inspector, won't actually contain anything
                return new OneToManyInspector(new OneToManyMapping());
            }
        }
    }
}