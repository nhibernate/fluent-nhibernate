using System;

namespace FluentNHibernate.MappingModel.ClassBased;

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
        if (obj.GetType() != typeof(ComponentType))
            return false;

        return Equals(obj as ComponentType);
    }

    public override string ToString()
    {
        return string.Format("ElementName: {0}", elementName);
    }

    public bool Equals(ComponentType other)
    {
        return Equals(other.elementName, elementName);
    }

    public override int GetHashCode()
    {
        return (elementName != null ? elementName.GetHashCode() : 0);
    }

    public static bool operator ==(ComponentType left, ComponentType right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ComponentType left, ComponentType right)
    {
        return !(left == right);
    }
}
