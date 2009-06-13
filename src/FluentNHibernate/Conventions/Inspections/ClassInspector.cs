using System;
using System.Reflection;
using FluentNHibernate.Conventions.DslImplementation;
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
        }

        public Type EntityType
        {
            get { return mapping.Type; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool LazyLoad
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

        public Cache Cache
        {
            get { throw new NotImplementedException(); }
        }

        public OptimisticLock OptimisticLock
        {
            get { throw new NotImplementedException(); }
        }
        public string Schema
        {
            get { throw new NotImplementedException(); }
        }

        public bool AutoImport
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
            return mapping.Attributes.IsSpecified(propertyMappings.Get(property));
        }
    }
}