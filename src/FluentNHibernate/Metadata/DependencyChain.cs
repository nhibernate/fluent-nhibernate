using System;
using System.Collections.Generic;
using System.Diagnostics;
using NHibernate;
using ShadeTree.Core;

namespace ShadeTree.DomainModel.Metadata
{
    public interface IDeleter
    {
        void DeleteAll(Type type);
        void FollowDependencies(TypeDependency dependency);
    }

    public class Deleter : IDeleter
    {
        private readonly ISession _session;
        private readonly List<Type> _types = new List<Type>();
        private readonly List<Type> _latches = new List<Type>();

        public Deleter(ISessionSource source)
        {
            _session = source.CreateSession();
        }

        #region IDeleter Members

        public void DeleteAll(Type type)
        {
            try
            {
                if (_types.Contains(type)) return;
                _session.Delete("from " + type.Name);
                _session.Flush();
                _types.Add(type);
            }
            catch (Exception e)
            {
                string message = string.Format("Error deleting {0}", type.FullName);
                Debug.WriteLine(message);
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        public void FollowDependencies(TypeDependency dependency)
        {
            if (!_latches.Contains(dependency.Type))
            {
                _latches.Add(dependency.Type);
                dependency.ForEach(d => d.DeleteAll(this));
            }
        }

        #endregion
    }


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