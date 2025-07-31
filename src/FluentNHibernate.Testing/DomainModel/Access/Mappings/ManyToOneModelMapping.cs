using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Access.Mappings;

class ManyToOneModelMapping : ClassMap<ManyToOneModel>
{
    public ManyToOneModelMapping()
    {
        Id(x => x.Id);
        References(x => x.Parent);
    }
}
