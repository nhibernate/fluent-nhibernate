using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class VersionInstance : VersionInspector, IVersionInstance
    {
        readonly VersionMapping mapping;
        bool nextBool = true;
        const int layer = Layer.Conventions;

        public VersionInstance(VersionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, layer, value)); }
        }

        public new IGeneratedInstance Generated
        {
            get { return new GeneratedInstance(value => mapping.Set(x => x.Generated, layer, value)); }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IVersionInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public void Column(string columnName)
        {
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Set(x => x.Name, layer, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }

        public new void UnsavedValue(string unsavedValue)
        {
            mapping.Set(x => x.UnsavedValue, layer, unsavedValue);
        }

        public new void Length(int length)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Length, layer, length);
        }

        public new void Precision(int precision)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Precision, layer, precision);
        }

        public new void Scale(int scale)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Scale, layer, scale);
        }

        public new void Nullable()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.NotNull, layer, !nextBool);

            nextBool = true;
        }

        public new void Unique()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Unique, layer, nextBool);

            nextBool = true;
        }

        public new void UniqueKey(string columns)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.UniqueKey, layer, columns);
        }

        public void CustomSqlType(string sqlType)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.SqlType, layer, sqlType);
        }

        public new void Index(string index)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Index, layer, index);
        }

        public new void Check(string constraint)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Check, layer, constraint);
        }

        public new void Default(object value)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Default, layer, value.ToString());
        }

        public void CustomType(string type)
        {
            mapping.Set(x => x.Type, layer, new TypeReference(type));
        }

        public void CustomType(Type type)
        {
            mapping.Set(x => x.Type, layer, new TypeReference(type));
        }

        public void CustomType<T>()
        {
            CustomType(typeof(T));
        }
    }
}