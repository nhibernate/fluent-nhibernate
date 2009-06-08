using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NHibernate.Properties;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class PropertyDsl : IPropertyInspector, IPropertyAlteration
    {
        private readonly PropertyMapping mapping;
        private readonly InspectorModelMapper<IPropertyInspector, PropertyMapping> propertyMappings = new InspectorModelMapper<IPropertyInspector, PropertyMapping>();

        public PropertyDsl(PropertyMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.Map(x => x.Insert, x => x.Insert);
            propertyMappings.Map(x => x.Update, x => x.Update);
            propertyMappings.Map(x => x.Type, x => x.Type);
            propertyMappings.Map(x => x.Access, x => x.Access);
            propertyMappings.Map(x => x.EntityType, x => x.ContainingEntityType);
            propertyMappings.Map(x => x.Formula, x => x.Formula);
            propertyMappings.Map(x => x.Name, x => x.Name);
            propertyMappings.Map(x => x.OptimisticLock, x => x.OptimisticLock);
            propertyMappings.Map(x => x.Generated, x => x.Generated);
        }

        /// <summary>
        /// Gets the requested value off the first column, as all columns are (currently) created equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetValueFromColumns<T>(Func<ColumnMapping, object> property)
        {
            var column = mapping.Columns.FirstOrDefault();

            if (column != null)
                return (T)property(column);

            return default(T);
        }

        #region Inspection

        bool IPropertyInspector.Insert
        {
            get { return mapping.Insert; }
        }

        bool IPropertyInspector.Update
        {
            get { return mapping.Update; }
        }

        int IPropertyInspector.Length
        {
            get { return GetValueFromColumns<int>(x => x.Length); }
        }

        bool IPropertyInspector.Nullable
        {
            get { return !GetValueFromColumns<bool>(x => x.NotNull); }
        }

        string IPropertyInspector.Formula
        {
            get { return mapping.Formula; }
        }

        TypeReference IPropertyInspector.Type
        {
            get { return mapping.Type; }
        }

        string IPropertyInspector.SqlType
        {
            get { return GetValueFromColumns<string>(x => x.SqlType); }
        }

        bool IPropertyInspector.Unique
        {
            get { return GetValueFromColumns<bool>(x => x.Unique); }
        }

        string IPropertyInspector.UniqueKey
        {
            get { return GetValueFromColumns<string>(x => x.UniqueKey); }
        }

        string IPropertyInspector.Name
        {
            get { return mapping.Name; }
        }

        bool IPropertyInspector.OptimisticLock
        {
            get { return mapping.OptimisticLock; }
        }

        string IPropertyInspector.Generated
        {
            get { return mapping.Generated; }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                foreach (var column in mapping.Columns)
                    yield return new ColumnDsl(mapping, column);
            }
        }

        Access IPropertyInspector.Access
        {
            get
            {
                if (mapping.Attributes.IsSpecified(x => x.Access))
                    return Access.FromString(mapping.Access);

                return Access.Unset;
            }
        }

        Type IInspector.EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        string IInspector.StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        bool IReadOnlyInspector.ReadOnly
        {
            get { return mapping.Insert && mapping.Update; }
        }

        PropertyInfo IExposedThroughPropertyInspector.Property
        {
            get { return mapping.PropertyInfo; }
        }

        bool IInspector.IsSet(PropertyInfo property)
        {
            return mapping.Attributes.IsSpecified(propertyMappings.Get(property));
        }

        #endregion

        #region Alteration

        private bool nextBool = true;

        void IInsertAlteration.Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
        }

        void IUpdateAlteration.Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
        }

        void IReadOnlyAlteration.ReadOnly()
        {
            mapping.Insert = mapping.Update = nextBool;
            nextBool = true;
        }

        void INullableAlteration.Nullable()
        {
            foreach (var column in mapping.Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        IAccessStrategyBuilder IAccessAlteration.Access
        {
            get { return new AccessMappingAlteration(mapping); }
        }

        void IPropertyAlteration.CustomTypeIs<T>()
        {
            mapping.Type = new TypeReference(typeof(T));
        }

        void IPropertyAlteration.CustomTypeIs(TypeReference type)
        {
            mapping.Type = type;
        }

        void IPropertyAlteration.CustomTypeIs(Type type)
        {
            mapping.Type = new TypeReference(type);
        }

        void IPropertyAlteration.CustomTypeIs(string type)
        {
            mapping.Type = new TypeReference(type);
        }

        void IPropertyAlteration.CustomSqlTypeIs(string sqlType)
        {
            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        void IPropertyAlteration.Unique()
        {
            foreach (var column in mapping.Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        void IPropertyAlteration.UniqueKey(string keyName)
        {
            foreach (var column in mapping.Columns)
                column.UniqueKey = keyName;
        }

        IPropertyAlteration IPropertyAlteration.Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        #endregion
    }
}