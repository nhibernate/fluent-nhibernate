using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

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

        protected override OneToManyPart<CHILD> HasMany<CHILD>(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.HasMany<CHILD>(property);
        }

        public void IgnoreProperty(Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
        }

        public override IIdentityPart Id(Expression<Func<T, object>> expression)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Id(expression);
        }

        protected override PropertyMap Map(PropertyInfo property, string columnName)
        {
            propertiesMapped.Add(property);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<OTHER> References<OTHER>(PropertyInfo property, string columnName)
        {
            propertiesMapped.Add(property);
            return base.References<OTHER>(property, columnName);
        }

        protected override ManyToManyPart<CHILD> HasManyToMany<CHILD>(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.HasManyToMany<CHILD>(property);
        }

        protected override ComponentPart<C> Component<C>(PropertyInfo property, Action<ComponentPart<C>> action)
        {
            propertiesMapped.Add(property);

            if (action == null)
                action = c => { };

            return base.Component(property, action);
        }

        public override IIdentityPart Id(System.Linq.Expressions.Expression<Func<T, object>> expression, string column)
        {
            propertiesMapped.Add(ReflectionHelper.GetProperty(expression));
            return base.Id(expression, column);
        }

        protected override OneToOnePart<OTHER> HasOne<OTHER>(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.HasOne<OTHER>(property);
        }

        protected override VersionPart Version(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.Version(property);
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