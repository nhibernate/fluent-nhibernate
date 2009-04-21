namespace FluentNHibernate.Mapping
{
    internal class DefaultAccessStrategyBuilder<T> : AccessStrategyBuilder<ClassMap<T>>
    {
        private readonly ClassMap<T> parent;
        private string accessValue;

        public DefaultAccessStrategyBuilder(ClassMap<T> parent) : base(parent)
        {
            this.parent = parent;
        }

        protected override void SetAccessAttribute(string value)
        {
            // HACK: fix to use HibernateMapping instead of hardcoded attributes
            accessValue = value;
        }

        public string GetValue()
        {
            return accessValue;
        }
    }
}