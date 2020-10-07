using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ParentPropertyPart
    {
        public ParentPropertyPart(ParentMapping mapping)
        {
            Access = new AccessStrategyBuilder<ParentPropertyPart>(this,
                value => mapping.Set(x => x.Access, Layer.UserSupplied, value));
        }

        public AccessStrategyBuilder<ParentPropertyPart> Access { get; }
    }
}