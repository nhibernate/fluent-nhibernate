using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToManyCollectionInstance : IOneToManyCollectionInspector, ICollectionInstance
    {
        new IOneToManyInstance Relationship { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        new IOneToManyCollectionInstance Not { get; }

        new IManyToOneInstance OtherSide { get; }

        /// <summary>
        /// Applies a filter to this relationship given its name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        /// <param name="condition">The condition to apply</param>
        void ApplyFilter(string name, string condition);

        /// <summary>
        /// Applies a filter to this relationship given its name.
        /// </summary>
        /// <param name="name">The filter's name</param>
        void ApplyFilter(string name);

        /// <summary>
        /// Applies a named filter to this relationship.
        /// </summary>
        /// <param name="condition">The condition to apply</param>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new();

        /// <summary>
        /// Applies a named filter to this relationship.
        /// </summary>
        /// <typeparam name="TFilter">
        /// The type of a <see cref="FilterDefinition"/> implementation
        /// defining the filter to apply.
        /// </typeparam>
        void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new();
    }
}