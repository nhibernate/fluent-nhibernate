namespace FluentNHibernate.Testing.DomainModel.Access;

class ManyToOneModel
{
    public virtual int Id { get; private set; }
    public virtual ParentModel Parent { get; private set; }
}
