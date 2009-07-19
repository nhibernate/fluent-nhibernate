using System;

namespace FluentNHibernate.Mapping
{
    public class PropertyGeneratedBuilder
    {
        private readonly PropertyMap parent;
        private readonly Action<string> setter;

        public PropertyGeneratedBuilder(PropertyMap parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public PropertyMap Never()
        {
            setter("never");
            return parent;
        }

        public PropertyMap Insert()
        {
            setter("insert");
            return parent;
        }

        public PropertyMap Always()
        {
            setter("always");
            return parent;
        }
    }
}