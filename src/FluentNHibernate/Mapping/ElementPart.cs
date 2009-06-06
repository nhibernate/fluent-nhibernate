using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class ElementPart
    {
        private readonly ElementMapping mapping = new ElementMapping();

        public ElementPart WithColumn(string elementColumnName)
        {
            mapping.AddColumn(new ColumnMapping { Name = elementColumnName });
            return this;
        }

        public ElementPart WithType<TElement>()
        {
            mapping.Type = typeof(TElement).AssemblyQualifiedName;
            return this;
        }

        public ElementMapping GetElementMapping()
        {
            return mapping;
        }
    }
}