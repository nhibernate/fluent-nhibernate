using System;

namespace FluentNHibernate.MappingModel.ClassBased
{
    [Serializable]
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

        public override bool Equals(object obj)
        {
            if (obj is ComponentType)
                return Equals(obj as ComponentType);
            return false;
        }

        public bool Equals(ComponentType other)
        {
            return Equals(other.elementName, elementName);
        }

        public override int GetHashCode()
        {
            return (elementName != null ? elementName.GetHashCode() : 0);
        }
    }
}