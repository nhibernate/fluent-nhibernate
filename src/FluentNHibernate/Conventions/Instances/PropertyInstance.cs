using System;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
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
            if (!mapping.IsSpecified(x => x.Insert))
            {
                mapping.Insert = nextBool;
                nextBool = true;
            }
        }

        public new void Update()
        {
            if (!mapping.IsSpecified(x => x.Update))
            {
                mapping.Update = nextBool;
                nextBool = true;
            }
        }

        public new void ReadOnly()
        {
            if (!mapping.IsSpecified(x => x.Insert) && !mapping.IsSpecified(x => x.Update))
            {
                mapping.Insert = mapping.Update = !nextBool;
                nextBool = true;
            }
        }

        public new void Nullable()
        {
            if (mapping.Columns.First().IsSpecified(x => x.NotNull))
                return;

            foreach (var column in mapping.Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Access))
                        mapping.Access = value;
                });
            }
        }

        public void CustomType<T>()
        {
            if (!mapping.IsSpecified(x => x.Type))
                mapping.Type = new TypeReference(typeof(T));
        }

        public void CustomType(TypeReference type)
        {
            if (!mapping.IsSpecified(x => x.Type))
                mapping.Type = type;
        }

        public void CustomType(Type type)
        {
            if (!mapping.IsSpecified(x => x.Type))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType(string type)
        {
            if (!mapping.IsSpecified(x => x.Type))
                mapping.Type = new TypeReference(type);
        }

        public void CustomSqlType(string sqlType)
        {
            if (mapping.Columns.First().IsSpecified(x => x.SqlType))
                return;
         
            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        public void Precision(int precision)
        {
            if (mapping.Columns.First().IsSpecified(x => x.Precision))
                return;

            foreach (var column in mapping.Columns)
                column.Precision = precision;
        }

        public void Scale(int scale)
        {
            if (mapping.Columns.First().IsSpecified(x => x.Scale))
                return;

            foreach (var column in mapping.Columns)
                column.Scale = scale;
        }

        public void Default(string value)
        {
            if (mapping.Columns.First().IsSpecified(x => x.Default))
                return;

            foreach (var column in mapping.Columns)
                column.Default = value;
        }

        public new void Unique()
        {
            if (mapping.Columns.First().IsSpecified(x => x.Unique))
                return;

            foreach (var column in mapping.Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        public new void UniqueKey(string keyName)
        {
            if (mapping.Columns.First().IsSpecified(x => x.UniqueKey))
                return;

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

        public void Column(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new void Formula(string formula)
        {
            if (!mapping.IsSpecified(x => x.Formula))
                mapping.Formula = formula;
        }

        public new IGeneratedInstance Generated
        {
            get
            {
                return new GeneratedInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Generated))
                        mapping.Generated = value;
                });
            }
        }

        public new void OptimisticLock()
        {
            if (!mapping.IsSpecified(x => x.OptimisticLock))
            {
                mapping.OptimisticLock = nextBool;
                nextBool = true;
            }
        }

        public new void Length(int length)
        {
            if (mapping.Columns.First().IsSpecified(x => x.Length))
                return;

            foreach (var column in mapping.Columns)
                column.Length = length;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified(x => x.Lazy))
            {
                mapping.Lazy = nextBool;
                nextBool = true;
            }
        }

        public new void Index(string value)
        {
            if (!mapping.IsSpecified(x => x.Index))
                mapping.Index = value;
        }
    }
}