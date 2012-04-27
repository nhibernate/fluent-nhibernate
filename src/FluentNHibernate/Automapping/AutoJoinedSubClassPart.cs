using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
#pragma warning disable 612,618
    public class AutoJoinedSubClassPart<T> : JoinedSubClassPart<T>, IAutoClasslike
#pragma warning restore 612,618
    {
        private readonly MappingProviderStore providers;
        private readonly IList<Member> mappedMembers = new List<Member>();

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

        internal override void OnMemberMapped(Member member)
        {
            mappedMembers.Add(member);
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
            return null;
        }

        public HibernateMapping GetHibernateMapping()
        {
            return null;
        }

        public IEnumerable<Member> GetIgnoredProperties()
        {
            return mappedMembers;
        }
    }
}