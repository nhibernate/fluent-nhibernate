using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    public interface IMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        bool IsSpecified(string attribute);
        void Set(string attribute, int layer, object value);
    }
}