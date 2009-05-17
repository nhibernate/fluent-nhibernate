using System;
using System.Reflection;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class ClassDsl : IClassInspector, IClassAlteration
    {
        private readonly ClassMapping mapping;
        private readonly InspectorModelMapper<IClassInspector, ClassMapping> propertyMappings = new InspectorModelMapper<IClassInspector, ClassMapping>();

        public ClassDsl(ClassMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.Map(x => x.EntityType, x => x.Type);
            propertyMappings.Map(x => x.TableName, x => x.TableName);
        }

        #region Inspection

        Type IInspector.EntityType
        {
            get { return mapping.Type; }
        }
        
        bool ILazyLoadInspector.LazyLoad
        {
            get { throw new NotImplementedException(); }
        }

        bool IReadOnlyInspector.ReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        string IClassInspector.TableName
        {
            get { throw new NotImplementedException(); }
        }
        
        Cache IClassInspector.Cache
        {
            get { throw new NotImplementedException(); }
        }
        
        OptimisticLock IClassInspector.OptimisticLock
        {
            get { throw new NotImplementedException(); }
        }
        string IClassInspector.Schema
        {
            get { throw new NotImplementedException(); }
        }
        
        bool IClassInspector.AutoImport
        {
            get { throw new NotImplementedException(); }
        }
        
        bool IClassInspector.DynamicUpdate
        {
            get { throw new NotImplementedException(); }
        }
        
        bool IClassInspector.DynamicInsert
        {
            get { throw new NotImplementedException(); }
        }

        int IClassInspector.BatchSize
        {
            get { throw new NotImplementedException(); }
        }

        bool IInspector.IsSet(PropertyInfo property)
        {
            return mapping.Attributes.IsSpecified(propertyMappings.Get(property));
        }

        #endregion

        #region Alteration

        void IClassAlteration.WithTable(string tableName)
        {
            mapping.TableName = tableName;
        }

        #endregion
    }
}