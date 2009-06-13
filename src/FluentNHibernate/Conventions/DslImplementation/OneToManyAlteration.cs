using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class OneToManyAlteration : IOneToManyAlteration
    {
        private readonly OneToManyMapping mapping;

        public OneToManyAlteration(OneToManyMapping mapping)
        {
            this.mapping = mapping;
        }
    }
}