using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class IdentityInstance : IdentityInspector, IIdentityInstance
    {
        private readonly IdMapping mapping;
        private bool nextBool = true;

        public IdentityInstance(IdMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
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

        public new void UnsavedValue(string unsavedValue)
        {
            if (!mapping.IsSpecified("UnsavedValue"))
                mapping.UnsavedValue = unsavedValue;
        }

        public new void Length(int length)
        {
            if (mapping.Columns.First().IsSpecified("Length"))
                return;

            foreach (var column in mapping.Columns)
                column.Length = length;
        }

        public void CustomType(string type)
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType(Type type)
        {
            if (!mapping.IsSpecified("Type"))
                mapping.Type = new TypeReference(type);
        }

        public void CustomType<T>()
        {
            CustomType(typeof(T));
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

        public IGeneratorInstance GeneratedBy
        {
            get
            {
                if (!mapping.IsSpecified("Generator"))
                    mapping.Generator = new GeneratorMapping();
                
                return new GeneratorInstance(mapping.Generator, mapping.Type.GetUnderlyingSystemType());
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IIdentityInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
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

        public new void Nullable()
        {
            if (mapping.Columns.First().IsSpecified("NotNull"))
                return;

            foreach (var column in mapping.Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        public new void Unique()
        {
            if (mapping.Columns.First().IsSpecified("Unique"))
                return;

            foreach (var column in mapping.Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        public new void UniqueKey(string columns)
        {
            if (mapping.Columns.First().IsSpecified("UniqueKey"))
                return;

            foreach (var column in mapping.Columns)
                column.UniqueKey = columns;
        }

        public void CustomSqlType(string sqlType)
        {
            if (mapping.Columns.First().IsSpecified("SqlType"))
                return;

            foreach (var column in mapping.Columns)
                column.SqlType = sqlType;
        }

        public new void Index(string index)
        {
            if (mapping.Columns.First().IsSpecified("Index"))
                return;

            foreach (var column in mapping.Columns)
                column.Index = index;
        }

        public new void Check(string constraint)
        {
            if (mapping.Columns.First().IsSpecified("Check"))
                return;

            foreach (var column in mapping.Columns)
                column.Check = constraint;
        }

        public new void Default(object value)
        {
            if (mapping.Columns.First().IsSpecified("Default"))
                return;

            foreach (var column in mapping.Columns)
                column.Default = value.ToString();
        }
    }
}