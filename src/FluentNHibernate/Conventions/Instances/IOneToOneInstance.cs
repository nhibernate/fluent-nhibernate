using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IOneToOneInstance : IOneToOneInspector
    {
        new IAccessInstance Access { get; }
        new ICascadeInstance Cascade { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IOneToOneInstance Not { get; }
        new IFetchInstance Fetch { get; }
        new void Class<T>();
        new void Class(Type type);
        new void Constrained();
        new void ForeignKey(string key);

        /// <summary>
        /// Specify the lazy behaviour of this relationship.
        /// </summary>
        /// <remarks>
        /// Defaults to Proxy lazy-loading. Use the <see cref="Not"/> modifier to disable
        /// lazy-loading, and use the <see cref="LazyLoad(FluentNHibernate.Mapping.Laziness)"/>
        /// overload to specify alternative lazy strategies.
        /// </remarks>
        /// <example>
        /// LazyLoad();
        /// Not.LazyLoad();
        /// </example>
        new void LazyLoad();

        /// <summary>
        /// Specify the lazy behaviour of this relationship. Cannot be used
        /// with the <see cref="Not"/> modifier.
        /// </summary>
        /// <param name="laziness">Laziness strategy</param>
        /// <example>
        /// LazyLoad(Laziness.NoProxy);
        /// </example>
        new void LazyLoad(Laziness laziness);
        new void PropertyRef(string propertyName);

        void OverrideInferredClass(Type type);
    }
}