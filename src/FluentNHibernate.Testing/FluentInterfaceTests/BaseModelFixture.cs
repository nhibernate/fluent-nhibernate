using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
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
    }
}