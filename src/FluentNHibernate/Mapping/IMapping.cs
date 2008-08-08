namespace FluentNHibernate.Mapping
{
    public interface IMapping
    {
        void ApplyMappings(IMappingVisitor visitor);
    }
}