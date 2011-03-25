using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public abstract class BaseModelFixture
    {
        protected ModelTester<ClassMap<T>, ClassMapping> ClassMap<T>()
        {
            return new ModelTester<ClassMap<T>, ClassMapping>(() => new ClassMap<T>(), x => ((IMappingProvider)x).GetClassMapping());
        }

        protected ModelTester<DiscriminatorPart, DiscriminatorMapping> DiscriminatorMap<T>()
        {
            return new ModelTester<DiscriminatorPart, DiscriminatorMapping>(() =>
            {
                return new DiscriminatorPart("column", typeof(T), (x, y) => {}, new TypeReference(typeof(object)));
            }, x => ((IDiscriminatorMappingProvider)x).GetDiscriminatorMapping());
        }

        protected ModelTester<SubClassPart<T>, SubclassMapping> Subclass<T>()
        {
            return new ModelTester<SubClassPart<T>, SubclassMapping>(() => new SubClassPart<T>(null, null), x => ((ISubclassMappingProvider)x).GetSubclassMapping());
        }

        protected ModelTester<SubclassMap<T>, SubclassMapping> SubclassMapForSubclass<T>()
        {
            return new ModelTester<SubclassMap<T>, SubclassMapping>(() => new SubclassMap<T>(), x => ((IIndeterminateSubclassMappingProvider)x).GetSubclassMapping(SubclassType.Subclass));
        }

        protected ModelTester<JoinedSubClassPart<T>, SubclassMapping> JoinedSubclass<T>()
        {
            return new ModelTester<JoinedSubClassPart<T>, SubclassMapping>(() => new JoinedSubClassPart<T>("column"), x => ((ISubclassMappingProvider)x).GetSubclassMapping());
        }

        protected ModelTester<SubclassMap<T>, SubclassMapping> SubclassMapForJoinedSubclass<T>()
        {
            return new ModelTester<SubclassMap<T>, SubclassMapping>(() => new SubclassMap<T>(), x => ((IIndeterminateSubclassMappingProvider)x).GetSubclassMapping(SubclassType.JoinedSubclass));
        }

        protected ModelTester<ComponentPart<T>, ComponentMapping> Component<T>()
        {
            return new ModelTester<ComponentPart<T>, ComponentMapping>(() => new ComponentPart<T>(typeof(ExampleClass), ReflectionHelper.GetMember<VersionTarget>(x => x.VersionNumber)), x => (ComponentMapping)((IComponentMappingProvider)x).GetComponentMapping());
        }

        protected ModelTester<DynamicComponentPart<T>, ComponentMapping> DynamicComponent<T>()
        {
            return new ModelTester<DynamicComponentPart<T>, ComponentMapping>(() => new DynamicComponentPart<T>(typeof(ExampleClass), ReflectionHelper.GetMember<VersionTarget>(x => x.VersionNumber)), x => (ComponentMapping)((IComponentMappingProvider)x).GetComponentMapping());
        }

        protected ModelTester<VersionPart, VersionMapping> Version()
        {
            return new ModelTester<VersionPart, VersionMapping>(() => new VersionPart(typeof(VersionTarget), ReflectionHelper.GetMember<VersionTarget>(x => x.VersionNumber)), x => ((IVersionMappingProvider)x).GetVersionMapping());
        }

        protected ModelTester<CachePart, CacheMapping> Cache()
        {
            return new ModelTester<CachePart, CacheMapping>(() => new CachePart(typeof(CachedRecord)), x => ((ICacheMappingProvider)x).GetCacheMapping());
        }

        protected ModelTester<IdentityPart, IdMapping> Id()
        {
            return new ModelTester<IdentityPart, IdMapping>(() => new IdentityPart(typeof(IdentityTarget), ReflectionHelper.GetMember<IdentityTarget>(x => x.IntId)), x => ((IIdentityMappingProvider)x).GetIdentityMapping());
        }

        protected ModelTester<CompositeIdentityPart<T>, CompositeIdMapping> CompositeId<T>()
        {
            return new ModelTester<CompositeIdentityPart<T>, CompositeIdMapping>(() => new CompositeIdentityPart<T>(), x => ((ICompositeIdMappingProvider)x).GetCompositeIdMapping());
        }

        protected ModelTester<OneToOnePart<PropertyReferenceTarget>, OneToOneMapping> OneToOne()
        {
            return new ModelTester<OneToOnePart<PropertyReferenceTarget>, OneToOneMapping>(() => new OneToOnePart<PropertyReferenceTarget>(typeof(PropertyTarget), ReflectionHelper.GetMember<PropertyTarget>(x => x.Reference)), x => ((IOneToOneMappingProvider)x).GetOneToOneMapping());
        }

        protected ModelTester<PropertyPart, PropertyMapping> Property()
        {
            return new ModelTester<PropertyPart, PropertyMapping>(() => new PropertyPart(ReflectionHelper.GetMember<PropertyTarget>(x => x.Name), typeof(PropertyTarget)), x => ((IPropertyMappingProvider)x).GetPropertyMapping());
        }

        protected ModelTester<PropertyPart, PropertyMapping> Property<T>(Expression<Func<T, object>> property)
        {
            return new ModelTester<PropertyPart, PropertyMapping>(() => new PropertyPart(ReflectionHelper.GetMember(property), typeof(T)), x => ((IPropertyMappingProvider)x).GetPropertyMapping());
        }

        protected ModelTester<OneToManyPart<T>, CollectionMapping> OneToMany<T>(Expression<Func<OneToManyTarget, IEnumerable<T>>> property)
        {
            return new ModelTester<OneToManyPart<T>, CollectionMapping>(() => new OneToManyPart<T>(typeof(OneToManyTarget), ReflectionHelper.GetMember(property)), x => ((ICollectionMappingProvider)x).GetCollectionMapping());
        }

        protected ModelTester<ManyToManyPart<T>, CollectionMapping> ManyToMany<T>(Expression<Func<ManyToManyTarget, IList<T>>> property)
        {
            return new ModelTester<ManyToManyPart<T>, CollectionMapping>(() => new ManyToManyPart<T>(typeof(ManyToManyTarget), ReflectionHelper.GetMember(property)), x => ((ICollectionMappingProvider)x).GetCollectionMapping());
        }

        protected ModelTester<ManyToManyPart<IDictionary>, CollectionMapping> ManyToMany(Expression<Func<ManyToManyTarget, IDictionary>> property)
        {
            return new ModelTester<ManyToManyPart<IDictionary>, CollectionMapping>(() => new ManyToManyPart<IDictionary>(typeof(ManyToManyTarget), ReflectionHelper.GetMember(property)), x => ((ICollectionMappingProvider)x).GetCollectionMapping());
        }

        protected ModelTester<ManyToManyPart<IDictionary<TIndex, TValue>>, CollectionMapping> ManyToMany<TIndex, TValue>(Expression<Func<ManyToManyTarget, IDictionary<TIndex, TValue>>> property)
        {
            return new ModelTester<ManyToManyPart<IDictionary<TIndex, TValue>>, CollectionMapping>(() => new ManyToManyPart<IDictionary<TIndex, TValue>>(typeof(ManyToManyTarget), ReflectionHelper.GetMember(property)), x => ((ICollectionMappingProvider)x).GetCollectionMapping());
        }

        protected ModelTester<ManyToOnePart<PropertyReferenceTarget>, ManyToOneMapping> ManyToOne()
        {
            return new ModelTester<ManyToOnePart<PropertyReferenceTarget>, ManyToOneMapping>(() => new ManyToOnePart<PropertyReferenceTarget>(typeof(PropertyTarget), ReflectionHelper.GetMember<PropertyTarget>(x => x.Reference)), x => ((IManyToOneMappingProvider)x).GetManyToOneMapping());
        }

        protected ModelTester<AnyPart<T>, AnyMapping> Any<T>()
        {
            return new ModelTester<AnyPart<T>, AnyMapping>(() => new AnyPart<T>(typeof(MappedObject), ReflectionHelper.GetMember<MappedObject>(x => x.Parent)), x => ((IAnyMappingProvider)x).GetAnyMapping());
        }

        protected ModelTester<JoinPart<T>, JoinMapping> Join<T>(string table)
        {
            return new ModelTester<JoinPart<T>, JoinMapping>(() => new JoinPart<T>(table), x => ((IJoinMappingProvider)x).GetJoinMapping());
        }

        protected ModelTester<HibernateMappingPart, HibernateMapping> HibernateMapping()
        {
            return new ModelTester<HibernateMappingPart, HibernateMapping>(() => new HibernateMappingPart(), x => ((IHibernateMappingProvider)x).GetHibernateMapping());
        }

        protected ModelTester<CompositeElementPart<T>, CompositeElementMapping> CompositeElement<T>()
        {
            return new ModelTester<CompositeElementPart<T>, CompositeElementMapping>(() => new CompositeElementPart<T>(typeof(MappedObject)), x => ((ICompositeElementMappingProvider)x).GetCompositeElementMapping());
        }

        protected ModelTester<StoredProcedurePart, StoredProcedureMapping> StoredProcedure()
        {
#pragma warning disable 612,618
            return new ModelTester<StoredProcedurePart, StoredProcedureMapping>(() => new StoredProcedurePart(null, null), x => x.GetStoredProcedureMapping());
#pragma warning restore 612,618
        }

        protected ModelTester<NaturalIdPart<T>, NaturalIdMapping> NaturalId<T>()
        {
            return new ModelTester<NaturalIdPart<T>, NaturalIdMapping>(() => new NaturalIdPart<T>(), x => ((INaturalIdMappingProvider)x).GetNaturalIdMapping());
        }
    }
}