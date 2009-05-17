using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
    public class PropertyDsl : IPropertyInspector
    {
        private readonly PropertyMapping mapping;
        private readonly IDictionary<string, Expression<Func<PropertyMapping, object>>> propertyMappings = new Dictionary<string, Expression<Func<PropertyMapping, object>>>();

        public PropertyDsl(PropertyMapping mapping)
        {
            this.mapping = mapping;

            Map(x => x.Insert, x => x.Insert);
            Map(x => x.Update, x => x.Update);
        }

        private void Map(Expression<Func<PropertyDsl, object>> dslProperty, Expression<Func<PropertyMapping, object>> mappingProperty)
        {
            propertyMappings.Add(ReflectionHelper.GetProperty(dslProperty).Name, mappingProperty);
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

        public bool Insert
        {
            get { return mapping.Insert; }
        }

        public bool Update
        {
            get { return mapping.Update; }
        }

        public int Length
        {
            get { return GetValueFromColumns<int>(x => x.Length); }
        }

        public bool Nullable
        {
            get { return !GetValueFromColumns<bool>(x => x.NotNull); }
        }

        public string Formula
        {
            get { return mapping.Formula; }
        }

        public string CustomType
        {
            get { return mapping.Type; }
        }

        public string SqlType
        {
            get { return GetValueFromColumns<string>(x => x.SqlType); }
        }

        public bool Unique
        {
            get { return GetValueFromColumns<bool>(x => x.Unique); }
        }

        public string UniqueKey
        {
            get { return GetValueFromColumns<string>(x => x.UniqueKey); }
        }

        public Access Access
        {
            get
            {
                if (mapping.Attributes.IsSpecified(x => x.Access))
                    return Access.FromString(mapping.Access);

                return Access.Unset;
            }
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }

        public bool ReadOnly
        {
            get { return mapping.Insert && mapping.Update; }
        }

        public Type PropertyType
        {
            get { return mapping.PropertyInfo.PropertyType; }
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.Attributes.IsSpecified(propertyMappings[property.Name]);
        }
    }
}