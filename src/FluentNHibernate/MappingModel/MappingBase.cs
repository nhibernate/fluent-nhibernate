namespace FluentNHibernate.MappingModel
{
    public abstract class MappingBase : IMappingBase
    {
        public abstract void AcceptVisitor(IMappingModelVisitor visitor);
        public abstract bool IsSpecified(string property);
    }
}