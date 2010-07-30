using System;
using System.Collections.Generic;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    class EntityUsingPrivateFields
    {
        int id;
        string one;
        DateTime two;
        DateTime? three;
        int _one;
        IList<EntityUsingPrivateFields> _children;

        public string PublicPropertyThatShouldBeIgnored { get; set; }
    }
}