using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances
{
    public class OneToManyInstance : OneToManyInspector, IOneToManyInstance
    {
        private readonly OneToManyMapping mapping;

        public OneToManyInstance(OneToManyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new INotFoundInstance NotFound
        {
            get
            {
                return new NotFoundInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.NotFound))
                        mapping.NotFound = value;
                });
            }
        }
    }
}