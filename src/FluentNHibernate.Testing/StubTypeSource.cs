using System;
using System.Collections.Generic;

namespace FluentNHibernate.Testing
{
    internal class StubTypeSource : ITypeSource
    {
        private readonly IEnumerable<Type> types;

        public StubTypeSource(params Type[] types)
        {
            this.types = types;
        }

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