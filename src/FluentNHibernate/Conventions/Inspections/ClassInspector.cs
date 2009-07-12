using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ClassInspector : IClassInspector
    {
        private readonly ClassMapping mapping;
        private readonly InspectorModelMapper<IClassInspector, ClassMapping> propertyMappings = new InspectorModelMapper<IClassInspector, ClassMapping>();

        public ClassInspector(ClassMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.Map(x => x.EntityType, x => x.Type);
            propertyMappings.Map(x => x.TableName, x => x.TableName);
            propertyMappings.Map(x => x.DynamicInsert, x => x.DynamicInsert);
            propertyMappings.Map(x => x.DynamicUpdate, x => x.DynamicUpdate);
            propertyMappings.Map(x => x.OptimisticLock, x => x.OptimisticLock);
            propertyMappings.Map(x => x.Cache, x => x.Cache);
        }

        public Type EntityType
        {
            get { return mapping.Type; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public Laziness LazyLoad
        {
            get { throw new NotImplementedException(); }
        }

        public bool ReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public string TableName
        {
            get { throw new NotImplementedException(); }
        }

        public ICacheInstance Cache
        {
            get
            {
                if (mapping.Cache == null)
                    // conventions are hitting it, user must want a cache
                    mapping.Cache = new CacheMapping();

                return new CacheInstance(mapping.Cache);
            }
        }

        public OptimisticLock OptimisticLock
        {
            get { throw new NotImplementedException(); }
        }
        public string Schema
        {
            get { throw new NotImplementedException(); }
        }

        public bool DynamicUpdate
        {
            get { throw new NotImplementedException(); }
        }

        public bool DynamicInsert
        {
            get { throw new NotImplementedException(); }
        }

        public int BatchSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
        }
    }
}