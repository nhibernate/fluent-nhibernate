using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;

namespace FluentNHibernate.Testing.Automapping
{
    /// <summary>
    /// Provides testing facilities for mapped entities within an <see cref="AutoPersistenceModel"/>.
    /// </summary>
    /// <typeparam name="T">The entity which mapping you want to test.</typeparam>
    public class AutoMappingTester<T> : MappingTester<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMappingTester&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public AutoMappingTester(AutoPersistenceModel mapper)
            : base(mapper)
        {
            ForMapping((ClassMap<T>)null);
        }
    }
}