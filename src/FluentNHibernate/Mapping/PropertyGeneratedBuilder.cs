using System;

namespace FluentNHibernate.Mapping
{
    public class PropertyGeneratedBuilder
    {
        private readonly PropertyPart parent;
        private readonly Action<string> setter;

        public PropertyGeneratedBuilder(PropertyPart parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public PropertyPart Never()
        {
            setter("never");
            return parent;
        }

        public PropertyPart Insert()
        {
            setter("insert");
            return parent;
        }

        public PropertyPart Always()
        {
            setter("always");
            return parent;
        }
    }
}