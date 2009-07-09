using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class OneToManyInstance : RelationshipInstance, IOneToManyInstance
    {
        public OneToManyInstance(ICollectionRelationshipMapping mapping)
            : base(mapping)
        {
        }
    }
}