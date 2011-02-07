using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
#pragma warning disable 612,618,672
    public class AutoJoinedSubClassPart<T> : JoinedSubClassPart<T>, IAutoClasslike
    {
        private readonly MappingProviderStore providers;
        private readonly IList<Member> membersMapped = new List<Member>();

        public AutoJoinedSubClassPart(string keyColumn)
            : this(keyColumn, new MappingProviderStore())
        {}

        AutoJoinedSubClassPart(string keyColumn, MappingProviderStore providers)
            : base(keyColumn, new AttributeStore(), providers)
        {
            this.providers = providers;
        }

        public object GetMapping()
        {
            return ((ISubclassMappingProvider)this).GetSubclassMapping();
        }

        void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
        {
            
        }

        void IAutoClasslike.AlterModel(ClassMappingBase mapping)
        {}

        protected override OneToManyPart<TChild> HasMany<TChild>(Member property)
        {
            membersMapped.Add(property);
            return base.HasMany<TChild>(property);
        }

        protected override PropertyPart Map(Member property, string columnName)
        {
            membersMapped.Add(property);
            return base.Map(property, columnName);
        }

        protected override ManyToOnePart<TOther> References<TOther>(Member property, string columnName)
        {
            membersMapped.Add(property);
            return base.References<TOther>(property, columnName);
        }

        protected override ManyToManyPart<TChild> HasManyToMany<TChild>(Member property)
        {
            membersMapped.Add(property);
            return base.HasManyToMany<TChild>(property);
        }

        protected override ComponentPart<TComponent> Component<TComponent>(Member property, Action<ComponentPart<TComponent>> action)
        {
            membersMapped.Add(property);

            if (action == null)
                action = c => { };

            return base.Component(property, action);
        }

        protected override OneToOnePart<TOther> HasOne<TOther>(Member property)
        {
            membersMapped.Add(property);
            return base.HasOne<TOther>(property);
        }

        public void JoinedSubClass<TSubclass>(string keyColumn, Action<AutoJoinedSubClassPart<TSubclass>> action)
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var joinedclass = (AutoJoinedSubClassPart<TSubclass>)Activator.CreateInstance(genericType, keyColumn);

            action(joinedclass);

            providers.Subclasses[typeof(TSubclass)] = joinedclass;
        }

        public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
        {
            var genericType = typeof(AutoJoinedSubClassPart<>).MakeGenericType(type);
            var joinedclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, keyColumn);

            providers.Subclasses[type] = joinedclass;

            return (IAutoClasslike)joinedclass;
        }

        public void SubClass<TSubclass>(string discriminatorValue, Action<AutoSubClassPart<TSubclass>> action)
        {
            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(typeof(TSubclass));
            var subclass = (AutoSubClassPart<TSubclass>)Activator.CreateInstance(genericType, discriminatorValue);

            action(subclass);

            providers.Subclasses[typeof(TSubclass)] = subclass;
        }

        public IAutoClasslike SubClass(Type type, string discriminatorValue)
        {
            var genericType = typeof(AutoSubClassPart<>).MakeGenericType(type);
            var subclass = (ISubclassMappingProvider)Activator.CreateInstance(genericType, discriminatorValue);

            providers.Subclasses[type] = subclass;

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

        public IEnumerable<Member> GetIgnoredProperties()
        {
            return membersMapped;
        }
    }
#pragma warning restore 612,618,672
}