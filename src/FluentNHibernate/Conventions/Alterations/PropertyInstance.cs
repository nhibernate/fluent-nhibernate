using System;
using System.Linq;
using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations
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
            get { return new AccessMappingAlteration(mapping); }
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

        public IPropertyAlteration Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public void ColumnName(string columnName)
        {
            var columnAttributes = mapping.Columns.First().Attributes.Clone();

            mapping.ClearColumns();
            mapping.AddColumn(new ColumnMapping(columnAttributes) { Name = columnName });
        }
    }
}