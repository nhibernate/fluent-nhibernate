using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{

    public class ComponentPart<T> : ComponentPartBase<T>, IComponent
    {
         public ComponentPart(PropertyInfo property)
            : this(new ComponentMapping(), property)
        {}

         public ComponentPart(ComponentMapping mapping, PropertyInfo property)
             : base(mapping, property)
         {
             SetAttribute("insert", "true");
             SetAttribute("update", "true");
         }
    }
}
