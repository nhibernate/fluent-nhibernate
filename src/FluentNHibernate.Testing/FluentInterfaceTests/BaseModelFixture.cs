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
using FluentNHibernate.Utils;
using Iesi.Collections.Generic;

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
            return new ModelTester<SubClassPart<T>, SubclassMapping>(() => new SubClassPart<T>(null, null), x => (SubclassMapping)((ISubclassMappingProvider)x).GetSubclassMapping());
        }

        protected ModelTester<SubclassMap<T>, SubclassMapping> SubclassMapForSubclass<T>()
        {
            return new ModelTester<SubclassMap<T>, SubclassMapping>(() => new SubclassMap<T>(), x => (SubclassMapping)((IIndeterminateSubclassMappingProvider)x).GetSubclassMapping(new SubclassMapping()));
        }

        protected ModelTester<JoinedSubClassPart<T>, JoinedSubclassMapping> JoinedSubclass<T>()
        {
            return new ModelTester<JoinedSubClassPart<T>, JoinedSubclassMapping>(() => new JoinedSubClassPart<T>("column"), x => (JoinedSubclassMapping)((ISubclassMappingProvider)x).GetSubclassMapping());
        }

        protected ModelTester<SubclassMap<T>, JoinedSubclassMapping> SubclassMapForJoinedSubclass<T>()
        {
            return new ModelTester<SubclassMap<T>, JoinedSubclassMapping>(() => new SubclassMap<T>(), x => (JoinedSubclassMapping)((IIndeterminateSubclassMappingProvider)x).GetSubclassMapping(new JoinedSubclassMapping()));
        }

        protected ModelTester<ComponentPart<T>, ComponentMapping> Component<T>()
        {
            return new ModelTester<ComponentPart<T>, ComponentMapping>(() => new ComponentPart<T>(typeof(ExampleClass), ReflectionHelper.GetProperty<VersionTarget>(x => x.VersionNumber)), x => (ComponentMapping)((IComponentMappingProvider)x).GetComponentMapping());
        }

        protected ModelTester<DynamicComponentPart<T>, DynamicComponentMapping> DynamicComponent<T>()
        {
            return new ModelTester<DynamicComponentPart<T>, DynamicComponentMapping>(() => new DynamicComponentPart<T>(typeof(ExampleClass), ReflectionHelper.GetProperty<VersionTarget>(x => x.VersionNumber)), x => (DynamicComponentMapping)((IComponentMappingProvider)x).GetComponentMapping());
        }

        protected ModelTester<VersionPart, VersionMapping> Version()
        {
            return new ModelTester<VersionPart, VersionMapping>(() => new VersionPart(typeof(VersionTarget), ReflectionHelper.GetProperty<VersionTarget>(x => x.VersionNumber)), x => ((IVersionMappingProvider)x).GetVersionMapping());
        }

        protected ModelTester<CachePart, CacheMapping> Cache()
        {
            return new ModelTester<CachePart, CacheMapping>(() => new CachePart(typeof(CachedRecord)), x => ((ICacheMappingProvider)x).GetCacheMapping());
        }

        protected ModelTester<IdentityPart, IdMapping> Id()
        {
            return new ModelTester<IdentityPart, IdMapping>(() => new IdentityPart(typeof(IdentityTarget), ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId)), x => ((IIdentityMappingProvider)x).GetIdentityMapping());
        }

        protected ModelTester<CompositeIdentityPart<T>, CompositeIdMapping> CompositeId<T>()
        {
            return new ModelTester<CompositeIdentityPart<T>, CompositeIdMapping>(() => new CompositeIdentityPart<T>(), x => ((ICompositeIdMappingProvider)x).GetCompositeIdMapping());
        }

        protected ModelTester<OneToOnePart<PropertyReferenceTarget>, OneToOneMapping> OneToOne()
        {
            return new ModelTester<OneToOnePart<PropertyReferenceTarget>, OneToOneMapping>(() => new OneToOnePart<PropertyReferenceTarget>(typeof(PropertyTarget), ReflectionHelper.GetProperty<PropertyTarget>(x => x.Reference)), x => ((IOneToOneMappingProvider)x).GetOneToOneMapping());
        }

        protected ModelTester<PropertyPart, PropertyMapping> Property()
        {
            return new ModelTester<PropertyPart, PropertyMapping>(() => new PropertyPart(ReflectionHelper.GetProperty<PropertyTarget>(x => x.Name), typeof(PropertyTarget)), x => ((IPropertyMappingProvider)x).GetPropertyMapping());
        }

        protected ModelTester<OneToManyPart<T>, ICollectionMapping> OneToMany<T>(Expression<Func<OneToManyTarget, IList<T>>> property)
        {
            return new ModelTester<OneToManyPart<T>, ICollectionMapping>(() => new OneToManyPart<T>(typeof(OneToManyTarget), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }

        protected ModelTester<OneToManyPart<T>, ICollectionMapping> OneToMany<T>(Expression<Func<OneToManyTarget, ISet<T>>> property)
        {
            return new ModelTester<OneToManyPart<T>, ICollectionMapping>(() => new OneToManyPart<T>(typeof(OneToManyTarget), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }

        protected ModelTester<OneToManyPart<T>, ICollectionMapping> OneToMany<T>(Expression<Func<OneToManyTarget, IDictionary<string, T>>> property)
        {
            return new ModelTester<OneToManyPart<T>, ICollectionMapping>(() => new OneToManyPart<T>(typeof(OneToManyTarget), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }

        protected ModelTester<ManyToManyPart<T>, ICollectionMapping> ManyToMany<T>(Expression<Func<ManyToManyTarget, IList<T>>> property)
        {
            return new ModelTester<ManyToManyPart<T>, ICollectionMapping>(() => new ManyToManyPart<T>(typeof(ManyToManyTarget), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }

        protected ModelTester<ManyToManyPart<IDictionary>, ICollectionMapping> ManyToMany(Expression<Func<ManyToManyTarget, IDictionary>> property)
        {
            return new ModelTester<ManyToManyPart<IDictionary>, ICollectionMapping>(() => new ManyToManyPart<IDictionary>(typeof(ManyToManyTarget), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }

        protected ModelTester<ManyToManyPart<IDictionary<TIndex, TValue>>, ICollectionMapping> ManyToMany<TIndex, TValue>(Expression<Func<ManyToManyTarget, IDictionary<TIndex, TValue>>> property)
        {
            return new ModelTester<ManyToManyPart<IDictionary<TIndex, TValue>>, ICollectionMapping>(() => new ManyToManyPart<IDictionary<TIndex, TValue>>(typeof(ManyToManyTarget), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }

        protected ModelTester<ManyToOnePart<PropertyReferenceTarget>, ManyToOneMapping> ManyToOne()
        {
            return new ModelTester<ManyToOnePart<PropertyReferenceTarget>, ManyToOneMapping>(() => new ManyToOnePart<PropertyReferenceTarget>(typeof(PropertyTarget), ReflectionHelper.GetProperty<PropertyTarget>(x => x.Reference)), x => ((IManyToOneMappingProvider)x).GetManyToOneMapping());
        }

        protected ModelTester<AnyPart<T>, AnyMapping> Any<T>()
        {
            return new ModelTester<AnyPart<T>, AnyMapping>(() => new AnyPart<T>(typeof(MappedObject), ReflectionHelper.GetProperty<MappedObject>(x => x.Parent)), x => ((IAnyMappingProvider)x).GetAnyMapping());
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
    }
}