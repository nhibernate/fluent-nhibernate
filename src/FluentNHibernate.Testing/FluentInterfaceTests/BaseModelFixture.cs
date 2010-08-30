using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Builders;
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
        protected ClassMapping MappingFor<T>(Action<ClassMap<T>> action) where T : Entity
        {
            var model = new PersistenceModel();
            var class_map = new ClassMap<T>();

            class_map.Id(x => x.Id);
            action(class_map);

            model.Add(class_map);
            
            return model.BuildMappings()
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(T));
        }

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
            return new ModelTester<JoinedSubClassPart<T>, SubclassMapping>(() => new JoinedSubClassPart<T>(), x => ((ISubclassMappingProvider)x).GetSubclassMapping());
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

        protected ModelTester<CacheBuilder, CacheMapping> Cache()
        {
            var mapping = new CacheMapping();
            return new ModelTester<CacheBuilder, CacheMapping>(() => new CacheBuilder(mapping, typeof(CachedRecord)), x => mapping);
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

        protected ModelTester<PropertyBuilder, PropertyMapping> Property()
        {
            var mapping = new PropertyMapping();
            return new ModelTester<PropertyBuilder, PropertyMapping>(() => new PropertyBuilder(mapping, typeof(PropertyTarget), ReflectionHelper.GetMember<PropertyTarget>(x => x.Name)), x => mapping);
        }

        protected ModelTester<PropertyBuilder, PropertyMapping> Property<T>(Expression<Func<T, object>> property)
        {
            var mapping = new PropertyMapping();
            return new ModelTester<PropertyBuilder, PropertyMapping>(() => new PropertyBuilder(mapping, typeof(T), ReflectionHelper.GetMember(property)), x => mapping);
        }

        protected ModelTester<ManyToOneBuilder<PropertyReferenceTarget>, ManyToOneMapping> ManyToOne()
        {
            var mapping = new ManyToOneMapping();
            return new ModelTester<ManyToOneBuilder<PropertyReferenceTarget>, ManyToOneMapping>(() => new ManyToOneBuilder<PropertyReferenceTarget>(mapping, typeof(PropertyTarget), ReflectionHelper.GetMember<PropertyTarget>(x => x.Reference)), x => mapping);
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

        protected ModelTester<CompositeElementBuilder<T>, CompositeElementMapping> CompositeElement<T>()
        {
            var mapping = new CompositeElementMapping();
            return new ModelTester<CompositeElementBuilder<T>, CompositeElementMapping>(() => new CompositeElementBuilder<T>(mapping, typeof(MappedObject)), x => mapping);
        }

        protected ModelTester<StoredProcedurePart, StoredProcedureMapping> StoredProcedure()
        {
            return new ModelTester<StoredProcedurePart, StoredProcedureMapping>(() => new StoredProcedurePart(null, null), x => x.GetStoredProcedureMapping());
        }

        protected ModelTester<NaturalIdPart<T>, NaturalIdMapping> NaturalId<T>()
        {
            return new ModelTester<NaturalIdPart<T>, NaturalIdMapping>(() => new NaturalIdPart<T>(), x => ((INaturalIdMappingProvider)x).GetNaturalIdMapping());
        }
    }
}