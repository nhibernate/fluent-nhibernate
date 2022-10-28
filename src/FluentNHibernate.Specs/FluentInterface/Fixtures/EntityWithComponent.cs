using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithComponent
    {
        public ComponentTarget Component { get; set; }
        public IDictionary DynamicComponent { get; set; }
        public IDictionary<string, object> GenericDynamicComponent { get; set; }
    }

    class EntityWithFieldComponent
    {
        public ComponentTarget Component;
        public IDictionary DynamicComponent;
        public IDictionary<string, object> GenericDynamicComponent;
    }

    class ComponentTarget
    {
    }
}