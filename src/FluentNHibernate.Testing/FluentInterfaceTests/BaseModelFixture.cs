using System;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public abstract class BaseModelFixture
    {
        protected ModelTester<ClassMap<T>, ClassMapping> ClassMap<T>()
        {
            return new ModelTester<ClassMap<T>, ClassMapping>(() => new ClassMap<T>(), x => x.GetClassMapping());
        }

        protected ModelTester<DiscriminatorPart, DiscriminatorMapping> DiscriminatorMap<T>()
        {
            return new ModelTester<DiscriminatorPart, DiscriminatorMapping>(() =>
            {
                var classMapping = new ClassMapping();
                var classMap = new ClassMap<T>(classMapping);
                return new DiscriminatorPart(classMap, classMapping, "column");
            }, x => x.GetDiscriminatorMapping());
        }

        protected ModelTester<SubClassPart<T>, SubclassMapping> SubClass<T>()
        {
            return new ModelTester<SubClassPart<T>, SubclassMapping>(() => new SubClassPart<T>(new SubclassMapping()), x => x.GetSubclassMapping());
        }

        protected ModelTester<JoinedSubClassPart<T>, JoinedSubclassMapping> JoinedSubClass<T>()
        {
            return new ModelTester<JoinedSubClassPart<T>, JoinedSubclassMapping>(() => new JoinedSubClassPart<T>(new JoinedSubclassMapping()), x => x.GetJoinedSubclassMapping());
        }

        protected ModelTester<ComponentPart<T>, ComponentMapping> Component<T>()
        {
            return new ModelTester<ComponentPart<T>, ComponentMapping>(() => new ComponentPart<T>(new ComponentMapping(), "prop"), x => (ComponentMapping)((IComponent)x).GetComponentMapping());
        }

        protected ModelTester<DynamicComponentPart<T>, DynamicComponentMapping> DynamicComponent<T>()
        {
            return new ModelTester<DynamicComponentPart<T>, DynamicComponentMapping>(() => new DynamicComponentPart<T>(new DynamicComponentMapping(), "prop"), x => (DynamicComponentMapping)((IDynamicComponent)x).GetComponentMapping());
        }

        protected ModelTester<IVersion, VersionMapping> Version()
        {
            return new ModelTester<IVersion, VersionMapping>(() => new VersionPart(typeof(VersionTarget), ReflectionHelper.GetProperty<VersionTarget>(x => x.VersionNumber)), x => x.GetVersionMapping());
        }

        protected ModelTester<ICache, CacheMapping> Cache()
        {
            return new ModelTester<ICache, CacheMapping>(() => new CachePart(), x => x.GetCacheMapping());
        }

        protected ModelTester<IIdentityPart, IdMapping> Id()
        {
            return new ModelTester<IIdentityPart, IdMapping>(() => new IdentityPart(typeof(IdentityTarget), ReflectionHelper.GetProperty<IdentityTarget>(x => x.IntId)), x => x.GetIdMapping());
        }

        protected ModelTester<OneToOnePart<PropertyReferenceTarget>, OneToOneMapping> OneToOne()
        {
            return new ModelTester<OneToOnePart<PropertyReferenceTarget>, OneToOneMapping>(() => new OneToOnePart<PropertyReferenceTarget>(typeof(PropertyTarget), ReflectionHelper.GetProperty<PropertyTarget>(x => x.Reference)), x => x.GetOneToOneMapping());
        }

        protected ModelTester<IProperty, PropertyMapping> Property()
        {
            return new ModelTester<IProperty, PropertyMapping>(() => new PropertyMap(ReflectionHelper.GetProperty<PropertyTarget>(x => x.Name), typeof(PropertyTarget)), x => x.GetPropertyMapping());
        }

        protected ModelTester<IOneToManyPart, ICollectionMapping> OneToMany<T>(Expression<Func<T, object>> property)
        {
            return new ModelTester<IOneToManyPart, ICollectionMapping>(() => new OneToManyPart<PropertyTarget>(typeof(T), ReflectionHelper.GetProperty(property)), x => x.GetCollectionMapping());
        }
    }
}