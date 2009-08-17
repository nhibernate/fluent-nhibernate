using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;

namespace FluentNHibernate.Testing.Automapping
{
    internal class StubTypeSource : ITypeSource
    {
        private readonly IEnumerable<Type> types;

        public StubTypeSource(Type type)
            : this(new[] { type })
        {}

        public StubTypeSource(IEnumerable<Type> types)
        {
            this.types = types;
        }

        public IEnumerable<Type> GetTypes()
        {
            return types;
        }
    }
}