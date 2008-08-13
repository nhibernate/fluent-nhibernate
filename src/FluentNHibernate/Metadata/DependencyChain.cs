using System;

namespace FluentNHibernate.Metadata
{
    public class DependencyChain
    {
        private readonly Cache<Type, TypeDependency> _dependencies;

        public DependencyChain()
        {
            _dependencies = new Cache<Type, TypeDependency>(type => new TypeDependency(type));
        }

        public void RegisterDependency(Type childType, Type parentType)
        {
            if (childType == parentType) return;

            TypeDependency child = _dependencies.Get(childType);
            TypeDependency parent = _dependencies.Get(parentType);

            parent.AddDependency(child);
        }

        public void DeleteAllOfType(IDeleter deleter, Type type)
        {
            _dependencies.Get(type).DeleteAll(deleter);
        }
    }
}
