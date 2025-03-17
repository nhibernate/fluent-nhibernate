using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping;

public class CachePart(Type entityType) : ICacheMappingProvider
{
    readonly AttributeStore attributes = new();

    /// <summary>
    /// Sets caching to read-write
    /// </summary>
    public CachePart ReadWrite() => CustomUsage("read-write");

    /// <summary>
    /// Sets caching to non-strict read-write
    /// </summary>
    public CachePart NonStrictReadWrite() => CustomUsage("nonstrict-read-write");

    /// <summary>
    /// Sets caching to read-only
    /// </summary>
    public CachePart ReadOnly() => CustomUsage("read-only");

    /// <summary>
    /// Sets caching to transactional
    /// </summary>
    public CachePart Transactional() => CustomUsage("transactional");

    /// <summary>
    /// Sets caching to never
    /// </summary>
    public CachePart Never() => CustomUsage("never");

    /// <summary>
    /// Specifies a custom cache behaviour
    /// </summary>
    /// <param name="custom">Custom behaviour</param>
    public CachePart CustomUsage(string custom)
    {
        attributes.Set("Usage", Layer.UserSupplied, custom);
        return this;
    }

    /// <summary>
    /// Specifies the cache region
    /// </summary>
    /// <returns></returns>
    public CachePart Region(string name)
    {
        attributes.Set("Region", Layer.UserSupplied, name);
        return this;
    }

    /// <summary>
    /// Include all properties for caching
    /// </summary>
    /// <returns></returns>
    public CachePart IncludeAll() => CustomInclude("all");

    /// <summary>
    /// Include only non-lazy properties for caching
    /// </summary>
    public CachePart IncludeNonLazy() => CustomInclude("non-lazy");

    /// <summary>
    /// Specify a custom property inclusion strategy
    /// </summary>
    /// <param name="custom">Inclusion strategy</param>
    public CachePart CustomInclude(string custom)
    {
        attributes.Set("Include", Layer.UserSupplied, custom);
        return this;
    }

    internal bool IsDirty => attributes.IsSpecified("Region") || attributes.IsSpecified("Usage") || attributes.IsSpecified("Include");

    CacheMapping ICacheMappingProvider.GetCacheMapping() =>
        new(attributes.Clone())
        {
            ContainedEntityType = entityType
        };
}
