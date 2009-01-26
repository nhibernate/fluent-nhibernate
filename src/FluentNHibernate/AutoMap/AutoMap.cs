using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoMap<T> : ClassMap<T>
    {
        private IList<PropertyInfo> propertiesMapped = new List<PropertyInfo>();
        private Dictionary<Type, object> joinedSubClasses = new Dictionary<Type, object>();

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

        public override IIdentityPart Id(System.Linq.Expressions.Expression<Func<T, object>> expression)
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

        public override ComponentPart<C> Component<C>(Expression<Func<T, object>> expression, Action<ComponentPart<C>> action)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));

            if (action == null)
                action = c => { };

            return base.Component(expression, action);
        }

        public override PropertyMap Map(System.Linq.Expressions.Expression<Func<T, object>> expression, string columnName)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Map(expression, columnName);
        }

        public override IIdentityPart Id(System.Linq.Expressions.Expression<Func<T, object>> expression, string column)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Id(expression, column);
        }

        public override ManyToOnePart<OTHER> References<OTHER>(System.Linq.Expressions.Expression<Func<T, OTHER>> expression, string columnName)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.References(expression, columnName);
        }

        public override OneToOnePart<OTHER> HasOne<OTHER>(System.Linq.Expressions.Expression<Func<T, OTHER>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.HasOne(expression);
        }

        public override VersionPart Version(System.Linq.Expressions.Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Version(expression);
        }

        public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);
            action(joinedclass);
            AddPart(joinedclass);
            joinedSubClasses.Add(typeof(TSubclass), joinedclass);
            return joinedclass;
        }

        public object JoinedSubClass(Type type, string keyColumn)
        {
            if (joinedSubClasses.ContainsKey(type))
                return joinedSubClasses[type];


            var genericType = typeof (AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (IMappingPart)Activator.CreateInstance(genericType, keyColumn);                      
            AddPart(joinedclass);
            joinedSubClasses.Add(type, joinedclass);
            return joinedclass;
        }


        public bool CanMapProperty(PropertyInfo property)
        {
            if (this is AutoJoinedSubClassPart<T>)
            {
                if (property.DeclaringType != typeof(T))
                    return false;
            }

            return true;
        }
    }
}