using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ComponentPartBase<T>, IComponent
    {
        public ComponentPart(Type entity, PropertyInfo property)
            : this(new ComponentMapping { ContainingEntityType = entity }, property.Name)
         {}

         private ComponentPart(ComponentMapping mapping, string propertyName)
             : base(mapping, propertyName)
         {
             Insert();
             Update();
         }

         public ComponentPart<T> Not
         {
             get
             {
                 var forceCall = ((IComponentBase)this).Not;
                 return this;
             }
         }

         public ComponentPart<T> ReadOnly()
         {
             ((IComponentBase)this).ReadOnly();
             return this;
         }

         public ComponentPart<T> Insert()
         {
             ((IComponentBase)this).Insert();
             return this;
         }

         public ComponentPart<T> Update()
         {
             ((IComponentBase)this).Update();
             return this;
         }
    }
}
