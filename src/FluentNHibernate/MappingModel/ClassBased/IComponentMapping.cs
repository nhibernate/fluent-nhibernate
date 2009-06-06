namespace FluentNHibernate.MappingModel.ClassBased
{
    public interface IComponentMapping
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
    }
}