namespace FluentNHibernate.Mapping
{
    internal class DefaultAccessStrategyBuilder<T> : AccessStrategyBuilder<ClassMap<T>>
    {
        private readonly ClassMap<T> parent;

        public DefaultAccessStrategyBuilder(ClassMap<T> parent) : base(parent)
        {
            this.parent = parent;
        }

        protected override void SetAccessAttribute(string value)
        {
            // forces the builder to set the attributes on the hibernate-mapping element
            // rather than the class
            parent.SetHibernateMappingAttribute("default-access", value);
        }
    }
}