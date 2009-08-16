using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class PropertyInspector : IPropertyInspector
    {
        private readonly InspectorModelMapper<IPropertyInspector, PropertyMapping> propertyMappings = new InspectorModelMapper<IPropertyInspector, PropertyMapping>();
        private readonly PropertyMapping mapping;

        public PropertyInspector(PropertyMapping mapping)
        {
            this.mapping = mapping;

            propertyMappings.AutoMap();
            propertyMappings.Map(x => x.LazyLoad, x => x.Lazy);
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

        public TypeReference Type
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

        public string Name
        {
            get { return mapping.Name; }
        }

        public bool OptimisticLock
        {
            get { return mapping.OptimisticLock; }
        }

        public Generated Generated
        {
            get { return Generated.FromString(mapping.Generated); }
        }

        public IDefaultableEnumerable<IColumnInspector> Columns
        {
            get
            {
                var items = new DefaultableList<IColumnInspector>();

                foreach (var column in mapping.Columns.UserDefined)
                    items.Add(new ColumnInspector(mapping.ContainingEntityType, column));

                return items;
            }
        }

        public string Index
        {
            get { return mapping.Index; }
        }

        public bool LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public Access Access
        {
            get
            {
                if (mapping.IsSpecified(x => x.Access))
                    return Access.FromString(mapping.Access);

                return Access.Unset;
            }
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool ReadOnly
        {
            get { return mapping.Insert && mapping.Update; }
        }

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public bool IsSet(PropertyInfo property)
        {
            return mapping.IsSpecified(propertyMappings.Get(property));
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
    }
}