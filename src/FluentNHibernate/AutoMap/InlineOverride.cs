using System;

namespace FluentNHibernate.AutoMap
{
    public class InlineOverride
    {
        private readonly Type type;
        private readonly Action<object> action;

        public InlineOverride(Type type, Action<object> action)
        {
            this.type = type;
            this.action = action;
        }

        public bool CanOverride(Type otherType)
        {
            return type == otherType || otherType.IsSubclassOf(type);
        }

        public void Apply(object mapping)
        {
            action(mapping);
        }
    }
}