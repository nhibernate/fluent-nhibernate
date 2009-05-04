using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IDiscriminatorPart
    {
        DiscriminatorMapping GetDiscriminatorMapping();
    }
}