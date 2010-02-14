using System;
using System.Collections;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithComponent
    {
        public ComponentTarget Component { get; set; }
        public IDictionary DynamicComponent { get; set; }
    }

    class EntityWithFieldComponent
    {
        public ComponentTarget Component;
        public IDictionary DynamicComponent;
    }

    class ComponentTarget
    {
    }
}