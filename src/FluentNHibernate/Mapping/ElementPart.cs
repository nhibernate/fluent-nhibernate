using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class ElementPart : IElementMappingProvider
    {
        readonly Type entity;
        readonly AttributeStore attributes = new AttributeStore();
        readonly AttributeStore columnAttributes = new AttributeStore();
        readonly ColumnMappingCollection<ElementPart> columns;

        public ElementPart(Type entity)
        {
            this.entity = entity;
            columns = new ColumnMappingCollection<ElementPart>(this);            
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
        public ColumnMappingCollection<ElementPart> Columns
        {
            get { return columns; }
        }

        /// <summary>
        /// Specify the element type
        /// </summary>
        /// <typeparam name="TElement">Element type</typeparam>
        public ElementPart Type<TElement>()
        {
            attributes.Set("Type", Layer.UserSupplied, new TypeReference(typeof(TElement)));
            return this;
        }

        /// <summary>
        /// Specify the element column length
        /// </summary>
        /// <param name="length">Column length</param>
        public ElementPart Length(int length)
        {
            columnAttributes.Set("Length", Layer.UserSupplied, length);
            return this;
        }

        /// <summary>
        /// Specify the element column formula
        /// </summary>
        /// <param name="formula">Formula</param>
        public ElementPart Formula(string formula)
        {
            attributes.Set("Formula", Layer.UserSupplied, formula);
            return this;
        }

        ElementMapping IElementMappingProvider.GetElementMapping()
        {
            var mapping = new ElementMapping(attributes.Clone());
            mapping.ContainingEntityType = entity;

            foreach (var column in Columns)
            {
                column.MergeAttributes(columnAttributes);
                mapping.AddColumn(Layer.Defaults, column);
            }

            return mapping;
        }
    }
}