using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NHibernate.UserTypes;

namespace FluentNHibernate.Conventions.Instances
{
    public class PropertyInstance : PropertyInspector, IPropertyInstance
    {
        private readonly PropertyMapping mapping;
        private bool nextBool = true;
        const int layer = Layer.Conventions;

        public PropertyInstance(PropertyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new void Insert()
        {
            mapping.Set(x => x.Insert, layer, nextBool);
            nextBool = true;
        }

        public new void Update()
        {
            mapping.Set(x => x.Update, layer, nextBool);
            nextBool = true;
        }

        public new void ReadOnly()
        {
            mapping.Set(x => x.Insert, layer, !nextBool);
            mapping.Set(x => x.Update, layer, !nextBool);
            nextBool = true;
        }

        public new void Nullable()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.NotNull, layer, !nextBool);

            nextBool = true;
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, layer, value)); }
        }

        public void CustomType(TypeReference type)
        {
            // Use "PropertyName_" as default prefix to avoid breaking existing code
            CustomType(type, Property.Name + "_");
        }

        public void CustomType(TypeReference type, string columnPrefix)
        {
            mapping.Set(x => x.Type, layer, type);

            if (typeof(ICompositeUserType).IsAssignableFrom(mapping.Type.GetUnderlyingSystemType()))
                AddColumnsForCompositeUserType(columnPrefix);
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
            foreach (var column in mapping.Columns)
                column.Set(x => x.SqlType, layer, sqlType);
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

        public new void Default(string value)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Default, layer, value);
        }

        public new void Unique()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Unique, layer, nextBool);

            nextBool = true;
        }

        public new void UniqueKey(string keyName)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.UniqueKey, layer, keyName);
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
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Set(x => x.Name, layer, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }

        public new void Formula(string formula)
        {
            mapping.Set(x => x.Formula, layer, formula);
            mapping.MakeColumnsEmpty(Layer.UserSupplied);
        }

        public new IGeneratedInstance Generated
        {
            get { return new GeneratedInstance(value => mapping.Set(x => x.Generated, layer, value)); }
        }

        public new void OptimisticLock()
        {
            mapping.Set(x => x.OptimisticLock, layer, nextBool);
            nextBool = true;
        }

        public new void Length(int length)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Length, layer, length);
        }

        public new void LazyLoad()
        {
            mapping.Set(x => x.Lazy, layer, nextBool);
            nextBool = true;
        }

        public new void Index(string value)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Index, layer, value);
        }

        public new void Check(string constraint)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Check, layer, constraint);
        }

        private void AddColumnsForCompositeUserType(string columnPrefix)
        {
            var inst = (ICompositeUserType)Activator.CreateInstance(mapping.Type.GetUnderlyingSystemType());

            if (inst.PropertyNames.Length > 1)
            {
                var existingColumn = mapping.Columns.Single();
                mapping.MakeColumnsEmpty(Layer.Conventions);

                foreach (var propertyName in inst.PropertyNames)
                {
                    var column = existingColumn.Clone();
                    column.Set(x => x.Name, layer, columnPrefix + propertyName);
                    mapping.AddColumn(Layer.Conventions, column);
                }
            }
        }
    }
}