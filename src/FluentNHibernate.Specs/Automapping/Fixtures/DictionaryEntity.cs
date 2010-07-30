using System.Collections;
using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    class DictionaryEntity
    {
        public int Id { get; set; }
        public IDictionary<string, EntityChild> GenericDictionary { get; set; }
        public IDictionary Dictionary { get; set; }
    }
}