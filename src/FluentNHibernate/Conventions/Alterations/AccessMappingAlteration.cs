using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Alterations
{
    internal class AccessMappingAlteration : AccessStrategyBuilder
    {
        public AccessMappingAlteration(PropertyMapping mapping)
            : base(value => mapping.Access = value)
        {}
    }
}