using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class AutoJoinedSubClassPart<T> : JoinedSubClassPart<T>, IAutoClasslike
    {
        private readonly IList<PropertyInfo> propertiesMapped = new List<PropertyInfo>();

        public AutoJoinedSubClassPart(string keyColumn)
            : base(keyColumn)
        {}

        public IEnumerable<PropertyInfo> PropertiesMapped
        {
            get { return propertiesMapped; }
        }

        public object GetMapping()
        {
            return GetJoinedSubclassMapping();
        }

        void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
        {
            
        }

        void IAutoClasslike.AlterModel(ClassMappingBase mapping)
        {}

        protected override OneToManyPart<TChild> HasMany<TChild>(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.HasMany<TChild>(property);
        }

        protected override PropertyMap Map(PropertyInfo property, string columnName)
        {
            propertiesMapped.Add(property);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(PropertyInfo property, string columnName)
        {
            propertiesMapped.Add(property);
            return base.References<TOther>(property, columnName);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.HasManyToMany<TChild>(property);
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            propertiesMapped.Add(property);

            if (action == null)
                action = c => { };

            return base.Component(property, action);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(PropertyInfo property)
        {
            propertiesMapped.Add(property);
            return base.HasOne<TOther>(property);
        }

        public void JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);

            action(joinedclass);

            joinedSubclasses.Add(joinedclass);
        }

        public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (IJoinedSubclass)Activator.CreateInstance(genericType, keyColumn);

            joinedSubclasses.Add(joinedclass);

            return (IAutoClasslike)joinedclass;
        }

        public void SubClass<TSubclass>(string discriminatorValue, Action<AutoSubClassPart<TSubclass>> action)
        {
            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var subclass = (AutoSubClassPart<TSubclass>)Activator.CreateInstance(genericType, discriminatorValue);

            action(subclass);

            subclasses.Add(subclass);
        }

        public IAutoClasslike SubClass(Type type, string discriminatorValue)
        {
            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(type);
            var subclass = (ISubclass)Activator.CreateInstance(genericType, discriminatorValue);

            subclasses.Add(subclass);

            return (IAutoClasslike)subclass;
        }

        public ClassMapping GetClassMapping()
        {
            throw new NotImplementedException();
        }

        public HibernateMapping GetHibernateMapping()
        {
            throw new NotImplementedException();
        }
    }
}