using System;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class PropertyInstance : PropertyInspector, IPropertyInstance
    {
        private readonly PropertyMapping mapping;
        private bool nextBool = true;

        public PropertyInstance(PropertyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new void Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
        }

        public new void ReadOnly()
        {
            mapping.Insert = mapping.Update = nextBool;
            nextBool = true;
        }

        public new void Nullable()
        {
            foreach (var column in mapping.Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        public new IAccessStrategyBuilder Access
        {
            get { return new AccessStrategyBuilder<PropertyInstance>(this, value => mapping.Access = value); }
        }

        public void CustomTypeIs<T>()
        {
            mapping.Type = new TypeReference(typeof(T));
        }

        public void CustomTypeIs(TypeReference type)
        {
            mapping.Type = type;
        }

        public void CustomTypeIs(Type type)
        {
            mapping.Type = new TypeReference(type);
        }

        public void CustomTypeIs(string type)
        {
            mapping.Type = new TypeReference(type);
        }

        public void CustomSqlTypeIs(string sqlType)
        {
            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        public new void Unique()
        {
            foreach (var column in mapping.Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        public new void UniqueKey(string keyName)
        {
            foreach (var column in mapping.Columns)
                column.UniqueKey = keyName;
        }

        public IPropertyInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public void ColumnName(string columnName)
        {
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }
    }
}