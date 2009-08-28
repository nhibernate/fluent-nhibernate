namespace FluentNHibernate.MappingModel
{
    public interface IMappingBase
    {
        void AcceptVisitor(IMappingModelVisitor visitor);
        bool IsSpecified(string property);
    }
}