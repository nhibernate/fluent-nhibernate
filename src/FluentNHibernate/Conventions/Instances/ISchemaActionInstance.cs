namespace FluentNHibernate.Conventions.Instances
{
    public interface ISchemaActionInstance
    {
        void None();
        void All();
        void Drop();
        void Update();
        void Validate();
        void Export();
        void Custom(string customValue);
    }
}