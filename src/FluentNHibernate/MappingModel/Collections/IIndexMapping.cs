namespace FluentNHibernate.MappingModel.Collections
{
    public interface IIndexMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        IDefaultableEnumerable<ColumnMapping> Columns { get; }
    }
}