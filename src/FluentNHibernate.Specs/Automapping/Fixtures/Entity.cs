using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    class Entity
    {
        public int Id { get; set; }
        public string One { get; set; }
        public TestEnum Enum { get; set; }
        public Entity Parent { get; set; }
        public IList<EntityChild> Children { get; set; }

        internal enum TestEnum {}
    }

    class EntityChild
    {}
}