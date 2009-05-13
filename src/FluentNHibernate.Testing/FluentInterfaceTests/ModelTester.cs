using System;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public class ModelTester<TFluentClass, TModel>
    {
        private readonly Func<TFluentClass> instantiatePart;
        private readonly Func<TFluentClass, TModel> getModel;
        private TFluentClass fluentClass;

        public ModelTester(Func<TFluentClass> instantiatePart, Func<TFluentClass, TModel> getModel)
        {
            this.instantiatePart = instantiatePart;
            this.getModel = getModel;
        }

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
}