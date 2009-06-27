using FluentNHibernate.Conventions.Alterations.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Alterations
{
    public class OneToManyInstance : OneToManyInspector, IOneToManyInstance
    {
        private readonly OneToManyMapping mapping;

        public OneToManyInstance(OneToManyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }
    }
}