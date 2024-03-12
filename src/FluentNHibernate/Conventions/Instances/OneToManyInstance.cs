using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Instances;

public class OneToManyInstance(OneToManyMapping mapping) : OneToManyInspector(mapping), IOneToManyInstance
{
    private readonly OneToManyMapping mapping = mapping;

    public new INotFoundInstance NotFound
    {
        get { return new NotFoundInstance(value => mapping.Set(x => x.NotFound, Layer.Conventions, value)); }
    }

    public void CustomClass<T>()
    {
        CustomClass(typeof(T));
    }

    public void CustomClass(Type type)
    {
        mapping.Set(x => x.Class, Layer.Conventions, new TypeReference(type));
    }
}
