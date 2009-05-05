using System;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class ModelTester<TFluentClass, TModel>
        where TFluentClass : new()
    {
        private readonly Func<TFluentClass, TModel> getModel;
        private TFluentClass fluentClass;

        public ModelTester(Func<TFluentClass, TModel> getModel)
        {
            this.getModel = getModel;
        }

        public ModelTester<TFluentClass, TModel> Mapping(Action<TFluentClass> action)
        {
            fluentClass = new TFluentClass();
            action(fluentClass);
            return this;
        }

        public void ModelShouldMatch(Action<TModel> action)
        {
            var model = getModel(fluentClass);
            action(model);
        }
    }
}