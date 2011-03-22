using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
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
                    if (!mapping.IsSpecified("NotFound"))
                        mapping.NotFound = value;
                });
            }
        }

        public void CustomClass<T>()
        {
            mapping.Class = new TypeReference(typeof(T));
        }

        public void CustomClass(Type type)
        {
            mapping.Class = new TypeReference(type);
        }
    }
}
