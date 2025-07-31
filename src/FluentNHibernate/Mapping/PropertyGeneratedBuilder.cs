using System;

namespace FluentNHibernate.Mapping;

public class PropertyGeneratedBuilder(PropertyPart parent, Action<string> setter)
{
    /// <summary>
    /// Property is never database generated
    /// </summary>
    public PropertyPart Never()
    {
        setter("never");
        return parent;
    }

    /// <summary>
    /// Property is only generated on insert
    /// </summary>
    public PropertyPart Insert()
    {
        setter("insert");
        return parent;
    }

    /// <summary>
    /// Property is always database generated
    /// </summary>
    public PropertyPart Always()
    {
        setter("always");
        return parent;
    }
}
