using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ComponentPartBase<T>
    {
        private readonly AccessStrategyBuilder<ComponentPart<T>> access;

        public ComponentPart(Type entity, PropertyInfo property)
            : this(new ComponentMapping { ContainingEntityType = entity }, property.Name)
         {}

         private ComponentPart(ComponentMapping mapping, string propertyName)
             : base(mapping, propertyName)
         {
             access = new AccessStrategyBuilder<ComponentPart<T>>(this, value => mapping.Access = value);

             Insert();
             Update();
         }

         /// <summary>
         /// Set the access and naming strategy for this component.
         /// </summary>
         public new AccessStrategyBuilder<ComponentPart<T>> Access
         {
             get { return access; }
         }

         public new ComponentPart<T> ParentReference(Expression<Func<T, object>> exp)
         {
             base.ParentReference(exp);
             return this;
         }

         public new ComponentPart<T> Not
         {
             get
             {
                 var forceExecution = base.Not;
                 return this;
             }
         }

         public new ComponentPart<T> ReadOnly()
         {
             base.ReadOnly();
             return this;
         }

         public new ComponentPart<T> Insert()
         {
             base.Insert();
             return this;
         }

         public new ComponentPart<T> Update()
         {
             base.Update();
             return this;
         }
    }
}
