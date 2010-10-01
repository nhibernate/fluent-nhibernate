using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface IIndexMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        TypeReference Type { get; set; }
    }
}