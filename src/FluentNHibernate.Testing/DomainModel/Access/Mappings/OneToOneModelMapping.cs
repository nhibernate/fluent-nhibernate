using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Access.Mappings;

class OneToOneModelMapping : ClassMap<OneToOneModel>
{
    public OneToOneModelMapping()
    {
        CompositeId().KeyReference(x => x.Parent);
    }
}
