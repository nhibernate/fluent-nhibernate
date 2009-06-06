using System;

namespace FluentNHibernate.Mapping
{
    public class PropertyGeneratedBuilder
    {
        private readonly IProperty parent;
        private readonly Action<string> setter;

        public PropertyGeneratedBuilder(IProperty parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        public IProperty Never()
        {
            setter("never");
            return parent;
        }

        public IProperty Insert()
        {
            setter("insert");
            return parent;
        }

        public IProperty Always()
        {
            setter("always");
            return parent;
        }
    }
}