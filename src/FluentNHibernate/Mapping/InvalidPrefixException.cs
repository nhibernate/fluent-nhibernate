using System;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Thrown when a prefix is specified for an access-strategy that isn't supported.
    /// </summary>
    public class InvalidPrefixException : Exception
    {
        public InvalidPrefixException(string message) : base(message)
        {}
    }
}