using System;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    ///// <summary>
    ///// Base class for attribute based conventions. Create a subclass of this to supply your own
    ///// attribute based conventions.
    ///// </summary>
    ///// <typeparam name="T">Attribute identifier</typeparam>
    //public abstract class AttributePropertyConvention<T> : IPropertyConvention
    //    where T : Attribute
    //{
    //    public bool Accept(IProperty target)
    //    {
    //        var attribute = Attribute.GetCustomAttribute(target.Property, typeof(T)) as T;

    //        return attribute != null;
    //    }

    //    public void Apply(IProperty target)
    //    {
    //        var attribute = Attribute.GetCustomAttribute(target.Property, typeof(T)) as T;
            
    //        Apply(attribute, target);
    //    }

    //    /// <summary>
    //    /// Apply changes to a property with an attribute matching T.
    //    /// </summary>
    //    /// <param name="attribute">Instance of attribute found on property.</param>
    //    /// <param name="target">Property with attribute</param>
    //    protected abstract void Apply(T attribute, IProperty target);
    //}
}