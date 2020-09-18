using System;
using System.Runtime.Serialization;

namespace FluentNHibernate.Infrastructure
{
    public static class Serialization
    {
        /// <summary>
        /// Extend <see cref="ISerializationSurrogate"/> with a "tester" method used by <see cref="SurrogateSelector"/>.
        /// Reference Via: https://github.com/CXuesong/BotBuilder.Standard/blob/netcore20%2Bnet45/CSharp/Library/Microsoft.Bot.Builder/Fibers/NetStandardSerialization.cs
        /// </summary>
        public interface ISurrogateProvider : ISerializationSurrogate
        {
            /// <summary>
            /// Determine whether this surrogate provider handles this type.
            /// </summary>
            /// <param name="type">The query type.</param>
            /// <param name="context">The serialization context.</param>
            /// <returns>True if this provider handles this type, false otherwise.</returns>
            bool Handles(Type type, StreamingContext context);
        }
    }
}
