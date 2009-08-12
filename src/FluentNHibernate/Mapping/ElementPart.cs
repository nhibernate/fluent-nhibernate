using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class ElementPart
    {
        private readonly Type entity;
        private readonly AttributeStore<ElementMapping> attributes = new AttributeStore<ElementMapping>();
        private IList<string> columns = new List<string>();

        public ElementPart(Type entity)
        {
            this.entity = entity;
        }

        public ElementPart Column(string elementColumnName)
        {
            columns.Add(elementColumnName);
            return this;
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

            columns.Each(x => mapping.AddColumn(new ColumnMapping { Name = x }));

            return mapping;
        }
    }
}