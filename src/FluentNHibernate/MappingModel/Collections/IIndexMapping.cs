namespace FluentNHibernate.MappingModel.Collections
{
    public interface IIndexMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
    }
}