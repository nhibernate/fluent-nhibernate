using System;
using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    internal class StubTypeSource : ITypeSource
    {
        private readonly IEnumerable<Type> types;

        public StubTypeSource(Type type)
            : this(new[] { type })
        { }

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
