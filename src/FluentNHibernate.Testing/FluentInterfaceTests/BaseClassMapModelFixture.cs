using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    public abstract class BaseClassMapModelFixture
    {
        protected ModelTester<ClassMap<T>, ClassMapping> ClassMap<T>()
        {
            return new ModelTester<ClassMap<T>, ClassMapping>(x => x.GetClassMapping());
        }
    }
}