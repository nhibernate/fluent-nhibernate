namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ComponentType
    {
        public static readonly ComponentType Component = new ComponentType("component");
        public static readonly ComponentType DynamicComponent = new ComponentType("dynamic-component");

        readonly string elementName;

        private ComponentType(string elementName)
        {
            this.elementName = elementName;
        }

        public string GetElementName()
        {
            return elementName;
        }
    }
}