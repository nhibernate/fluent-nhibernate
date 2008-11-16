using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMap<T> : ClassMap<T>
    {
        private IList<PropertyInfo> propertiesMapped = new List<PropertyInfo>();
        public IList<PropertyInfo> PropertiesMapped
        {
            get { return propertiesMapped; }
            set { propertiesMapped = value; }
        }

        public override OneToManyPart<T, CHILD> HasMany<CHILD>(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.HasMany<CHILD>(expression);
        }

        public void IgnoreProperty(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
        }

        public override IdentityPart Id(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Id(expression);
        }

        public override PropertyMap Map(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Map(expression);
        }

        public override ManyToOnePart<OTHER> References<OTHER>(System.Linq.Expressions.Expression<Func<T, OTHER>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.References(expression);
        }

        public override ManyToManyPart<T, CHILD> HasManyToMany<CHILD>(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.HasManyToMany<CHILD>(expression);
        }

        public override ComponentPart<C> Component<C>(System.Linq.Expressions.Expression<Func<T, object>> expression, Action<ComponentPart<C>> action)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Component(expression, action);
        }

        public override PropertyMap Map(System.Linq.Expressions.Expression<Func<T, object>> expression, string columnName)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Map(expression, columnName);
        }

        public override IdentityPart Id(System.Linq.Expressions.Expression<Func<T, object>> expression, string column)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Id(expression, column);
        }

        public override ManyToOnePart<OTHER> References<OTHER>(System.Linq.Expressions.Expression<Func<T, OTHER>> expression, string columnName)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.References(expression, columnName);
        }

        public override VersionPart Version(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Version(expression);
        }
    }
}