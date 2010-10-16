using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToOneInstance : IManyToOneInspector
    {
        void Column(string columnName);
        void CustomClass<T>();
        void CustomClass(Type type);
        new IAccessInstance Access { get; }
        new ICascadeInstance Cascade { get; }
        new IFetchInstance Fetch { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IManyToOneInstance Not { get; }
        new INotFoundInstance NotFound { get; }
        void Index(string index);
        new void Insert();
        new void OptimisticLock();

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

        new void Nullable();
        new void PropertyRef(string property);
        void ReadOnly();
        void Unique();
        void UniqueKey(string key);
        new void Update();
        new void ForeignKey(string key);
        new void Formula(string formula);

        void OverrideInferredClass(Type type);
    }
}