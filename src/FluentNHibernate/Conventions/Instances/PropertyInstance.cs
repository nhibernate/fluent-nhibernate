using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NHibernate.Type;
using NHibernate.UserTypes;

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
            if (!mapping.IsSpecified("Insert"))
                mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void Update()
        {
            if (!mapping.IsSpecified("Update"))
                mapping.Update = nextBool;
            nextBool = true;
        }

        public new void ReadOnly()
        {
            if (!mapping.IsSpecified("Insert") && !mapping.IsSpecified("Update"))
                mapping.Insert = mapping.Update = !nextBool;
            nextBool = true;
        }

        public new void Nullable()
        {
            if (!mapping.Columns.First().IsSpecified("NotNull"))
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
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }

        public void CustomType(TypeReference type)
        {
            // Use "PropertyName_" as default prefix to avoid breaking existing code
            CustomType(type, Property.Name + "_");
        }

        public void CustomType(TypeReference type, string columnPrefix)
        {
            if (!mapping.IsSpecified("Type"))
            {
                mapping.Type = type;

                if (typeof(ICompositeUserType).IsAssignableFrom(mapping.Type.GetUnderlyingSystemType()))
                    AddColumnsForCompositeUserType(columnPrefix);
            }
        }

        public void CustomType<T>(string columnPrefix)
        {
            CustomType(typeof(T), columnPrefix);
        }

        public void CustomType<T>()
        {
            CustomType(typeof(T));
        }

        public void CustomType(Type type)
        {
            CustomType(new TypeReference(type));
        }

        public void CustomType(Type type, string columnPrefix)
        {
            CustomType(new TypeReference(type), columnPrefix);
        }

        public void CustomType(string type)
        {
            CustomType(new TypeReference(type));
        }
        
        public void CustomType(string type, string columnPrefix)
        {
            CustomType(new TypeReference(type), columnPrefix);
        }

        public void CustomSqlType(string sqlType)
        {
            if (mapping.Columns.First().IsSpecified("SqlType"))
                return;
         
            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        public new void Precision(int precision)
        {
            if (mapping.Columns.First().IsSpecified("Precision"))
                return;

            foreach (var column in mapping.Columns)
                column.Precision = precision;
        }

        public new void Scale(int scale)
        {
            if (mapping.Columns.First().IsSpecified("Scale"))
                return;

            foreach (var column in mapping.Columns)
                column.Scale = scale;
        }

        public new void Default(string value)
        {
            if (mapping.Columns.First().IsSpecified("Default"))
                return;

            foreach (var column in mapping.Columns)
                column.Default = value;
        }

        public new void Unique()
        {
            if (!mapping.Columns.First().IsSpecified("Unique"))
                foreach (var column in mapping.Columns)
                    column.Unique = nextBool;

            nextBool = true;
        }

        public new void UniqueKey(string keyName)
        {
            if (mapping.Columns.First().IsSpecified("UniqueKey"))
                return;

            foreach (var column in mapping.Columns)
                column.UniqueKey = keyName;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new void Formula(string formula)
        {
            if (!mapping.IsSpecified("Formula"))
            {
                mapping.Formula = formula;
                mapping.ClearColumns();
            }
        }

        public new IGeneratedInstance Generated
        {
            get
            {
                return new GeneratedInstance(value =>
                {
                    if (!mapping.IsSpecified("Generated"))
                        mapping.Generated = value;
                });
            }
        }

        public new void OptimisticLock()
        {
            if (!mapping.IsSpecified("OptimisticLock"))
                mapping.OptimisticLock = nextBool;
            nextBool = true;
        }

        public new void Length(int length)
        {
            if (mapping.Columns.First().IsSpecified("Length"))
                return;

            foreach (var column in mapping.Columns)
                column.Length = length;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
                mapping.Lazy = nextBool;
            nextBool = true;
        }

        public new void Index(string value)
        {
            if (mapping.Columns.First().IsSpecified("Index"))
                return;

            foreach (var column in mapping.Columns)
                column.Index = value;
        }

        public new void Check(string constraint)
        {
            if (mapping.Columns.First().IsSpecified("Check"))
                return;

            foreach (var column in mapping.Columns)
                column.Check = constraint;
        }

        private void AddColumnsForCompositeUserType(string columnPrefix)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(mapping.Type.GetUnderlyingSystemType());

            if (inst.PropertyNames.Length > 1)
            {
                var existingColumn = mapping.Columns.Single();
                mapping.ClearColumns();

                foreach (var propertyName in inst.PropertyNames)
                {
                    var column = existingColumn.Clone();
                    column.Name = columnPrefix + propertyName;
                    mapping.AddColumn(column);
                }
            }
        }
    }
}