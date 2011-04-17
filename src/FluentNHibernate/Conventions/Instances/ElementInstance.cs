using System;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class ElementInstance : ElementInspector, IElementInstance
    {
        private readonly ElementMapping mapping;

        public ElementInstance(ElementMapping mapping)
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

        public new void Type<T>()
        {
            Type(typeof(T));
        }

        public new void Type(string type)
        {
            mapping.Set(x => x.Type, Layer.Conventions, new TypeReference(type));
        }

        public new void Type(Type type)
        {
            mapping.Set(x => x.Type, Layer.Conventions, new TypeReference(type));
        }
    }
}