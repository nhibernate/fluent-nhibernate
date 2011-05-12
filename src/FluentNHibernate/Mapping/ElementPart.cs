using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class ElementPart : IElementMappingProvider
    {
        Type entity;
        AttributeStore<ElementMapping> attributes = new AttributeStore<ElementMapping>();
        AttributeStore<ColumnMapping> columnAttributes = new AttributeStore<ColumnMapping>();

        public ElementPart(Type entity)
        {
            this.entity = entity;
            Columns = new ColumnMappingCollection<ElementPart>(this);            
        }

        /// <summary>
        /// Specify the element column name
        /// </summary>
        /// <param name="elementColumnName">Column name</param>
        public ElementPart Column(string elementColumnName)
        {
            Columns.Add(elementColumnName);
            return this;
        }

        /// <summary>
        /// Modify the columns for this element
        /// </summary>
        public ColumnMappingCollection<ElementPart> Columns { get; private set; }

        /// <summary>
        /// Specify the element type
        /// </summary>
        /// <typeparam name="TElement">Element type</typeparam>
        public ElementPart Type<TElement>()
        {
            attributes.Set(x => x.Type, new TypeReference(typeof(TElement)));
            return this;
        }

        /// <summary>
        /// Specify the element column length
        /// </summary>
        /// <param name="length">Column length</param>
        public ElementPart Length(int length)
        {
            columnAttributes.Set(x => x.Length, length);
            return this;
        }

        /// <summary>
        /// Specify the element column formula
        /// </summary>
        /// <param name="formula">Formula</param>
        public ElementPart Formula(string formula)
        {
            attributes.Set(x => x.Formula, formula);
            return this;
        }

        ElementMapping IElementMappingProvider.GetElementMapping()
        {
            var mapping = new ElementMapping(attributes.CloneInner());
            mapping.ContainingEntityType = entity;

            foreach (var column in Columns)
            {
                column.MergeAttributes(columnAttributes);
                mapping.AddColumn(column);
            }

            return mapping;
        }
    }
}