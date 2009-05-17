namespace FluentNHibernate.Mapping
{
    internal class DefaultAccessStrategyBuilder<T> : AccessStrategyBuilder<ClassMap<T>>
    {
        private string accessValue;

        public DefaultAccessStrategyBuilder(ClassMap<T> parent)
            : base(parent)
        {
            setValue = value => accessValue = value;
        }

        public string GetValue()
        {
            return accessValue;
        }
    }
}