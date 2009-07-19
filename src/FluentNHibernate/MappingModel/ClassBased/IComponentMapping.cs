namespace FluentNHibernate.MappingModel.ClassBased
{
    public interface IComponentMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        ParentMapping Parent { get; set; }
        bool Insert { get; set; }
        bool Update { get; set; }
        string Access { get; set; }
    }
}