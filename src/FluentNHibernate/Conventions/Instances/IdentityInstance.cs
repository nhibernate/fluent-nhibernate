using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
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
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Set(x => x.Name, Layer.Conventions, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }

        public new void UnsavedValue(string unsavedValue)
        {
            mapping.Set(x => x.UnsavedValue, Layer.Conventions, unsavedValue);
        }

        public new void Length(int length)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Length, Layer.Conventions, length);
        }

        public void CustomType(string type)
        {
            mapping.Set(x => x.Type, Layer.Conventions, new TypeReference(type));
        }

        public void CustomType(Type type)
        {
            mapping.Set(x => x.Type, Layer.Conventions, new TypeReference(type));
        }

        public void CustomType<T>()
        {
            CustomType(typeof(T));
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public IGeneratorInstance GeneratedBy
        {
            get
            {
                mapping.Set(x => x.Generator, Layer.Conventions, new GeneratorMapping());
                
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
            foreach (var column in mapping.Columns)
                column.Set(x => x.Precision, Layer.Conventions, precision);
        }

        public new void Scale(int scale)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Scale, Layer.Conventions, scale);
        }

        public new void Nullable()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.NotNull, Layer.Conventions, !nextBool);

            nextBool = true;
        }

        public new void Unique()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Unique, Layer.Conventions, nextBool);

            nextBool = true;
        }

        public new void UniqueKey(string columns)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.UniqueKey, Layer.Conventions, columns);
        }

        public void CustomSqlType(string sqlType)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.SqlType, Layer.Conventions, sqlType);
        }

        public new void Index(string index)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Index, Layer.Conventions, index);
        }

        public new void Check(string constraint)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Check, Layer.Conventions, constraint);
        }

        public new void Default(object value)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Default, Layer.Conventions, value.ToString());
        }
    }
}