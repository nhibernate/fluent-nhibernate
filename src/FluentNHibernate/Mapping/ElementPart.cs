using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class ElementPart
    {
        private readonly Type entity;
        private readonly AttributeStore<ElementMapping> attributes = new AttributeStore<ElementMapping>();
        private readonly ColumnMappingCollection<ElementPart> columns;

        public ElementPart(Type entity)
        {
            this.entity = entity;
            columns = new ColumnMappingCollection<ElementPart>(this);            
        }

        public ElementPart Column(string elementColumnName)
        {
            columns.Add(elementColumnName);
            return this;
        }

        public ColumnMappingCollection<ElementPart> Columns
        {
            get { return columns; }
        }

        public ElementPart Type<TElement>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(TElement)));
            return this;
        }

        public ElementMapping GetElementMapping()
        {
            var mapping = new ElementMapping(attributes.CloneInner());
            mapping.ContainingEntityType = entity;

            foreach (var column in Columns)
                mapping.AddColumn(column);

            return mapping;
        }

        public ElementPart Length(int length)
        {
            attributes.Set(x => x.Length, length);
            return this;
        }

        public ElementPart Formula(string formula)
        {
            attributes.Set(x => x.Formula, formula);
            return this;
        }
    }
}