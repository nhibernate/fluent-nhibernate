using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class ElementPart
    {
        private readonly ElementMapping mapping = new ElementMapping();
        private readonly Type entity;

        public ElementPart(Type entity)
        {
            this.entity = entity;
        }

        public ElementPart WithColumn(string elementColumnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = elementColumnName });
            return this;
        }

        public ElementPart WithType<TElement>()
        {
            mapping.Type = new TypeReference(typeof(TElement));
            return this;
        }

        public ElementMapping GetElementMapping()
        {
            mapping.ContainingEntityType = entity;
            return mapping;
        }
    }
}