using System;

namespace FluentNHibernate.Testing.FluentInterfaceTests;

public class ModelTester<TFluentClass, TModel>(Func<TFluentClass> instantiatePart, Func<TFluentClass, TModel> getModel)
{
    TFluentClass fluentClass;

    public ModelTester<TFluentClass, TModel> Mapping(Action<TFluentClass> action)
    {
        fluentClass = instantiatePart();
        action(fluentClass);
        return this;
    }

    public void ModelShouldMatch(Action<TModel> action)
    {
        var model = getModel(fluentClass);
        action(model);
    }
}
