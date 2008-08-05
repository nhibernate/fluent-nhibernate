using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentNHibernate.Metadata
{
    public class TypeDependency
    {
        private readonly List<TypeDependency> _dependencies = new List<TypeDependency>();
        private readonly Type _type;

        public TypeDependency(Type type)
        {
            _type = type;
        }

        public Type Type
        {
            get { return _type; }
        }

        public void AddDependency(TypeDependency dependency)
        {
            if (_dependencies.Contains(dependency)) return;
            _dependencies.Add(dependency);
        }

        public void DeleteAll(IDeleter deleter)
        {
            deleter.FollowDependencies(this);
            
            Debug.WriteLine("delete:  " + _type.FullName);

            deleter.DeleteAll(_type);
        }

        public void ForEach(Action<TypeDependency> action)
        {
            _dependencies.ForEach(action);
        }

        public bool Equals(TypeDependency obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj._type, _type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TypeDependency)) return false;
            return Equals((TypeDependency) obj);
        }

        public override int GetHashCode()
        {
            return (_type != null ? _type.GetHashCode() : 0);
        }
    }
}