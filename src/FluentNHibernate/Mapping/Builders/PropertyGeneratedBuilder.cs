using System;

namespace FluentNHibernate.Mapping.Builders
{
    public class PropertyGeneratedBuilder
    {
        private readonly PropertyBuilder parent;
        private readonly Action<string> setter;

        public PropertyGeneratedBuilder(PropertyBuilder parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        /// <summary>
        /// Property is never database generated
        /// </summary>
        public PropertyBuilder Never()
        {
            setter("never");
            return parent;
        }

        /// <summary>
        /// Property is only generated on insert
        /// </summary>
        public PropertyBuilder Insert()
        {
            setter("insert");
            return parent;
        }

        /// <summary>
        /// Property is always database generated
        /// </summary>
        public PropertyBuilder Always()
        {
            setter("always");
            return parent;
        }
    }
}