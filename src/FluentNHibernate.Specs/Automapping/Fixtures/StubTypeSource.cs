using System;
using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    internal class StubTypeSource : ITypeSource
    {
        private readonly IEnumerable<Type> types;

        public StubTypeSource(params Type[] types)
        {
            this.types = types;
        }

        public IEnumerable<Type> GetTypes()
        {
            return types;
        }
    }
}
