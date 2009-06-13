using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public class TypeReference
    {
        private readonly Type innerType;
        private readonly string innerName;

        public TypeReference(string name)
        {
            innerType = Type.GetType(name, false, true);
            innerName = name;
        }

        public TypeReference(Type type)
        {
            innerType = type;
            innerName = type.Name;
        }

        public string Name
        {
            get { return innerName; }
        }

        public bool IsEnum
        {
            get
            {
                if (innerType == null)
                    return false;

                return innerType.IsEnum;
            }
        }

        public bool IsGenericType
        {
            get
            {
                if (innerType == null)
                    return false;

                return innerType.IsGenericType;
            }
        }

        public bool IsGenericTypeDefinition
        {
            get
            {
                if (innerType == null)
                    return false;

                return innerType.IsGenericTypeDefinition;
            }
        }

        public Type GetGenericTypeDefinition()
        {
            if (innerType == null)
                return null;

            return innerType.GetGenericTypeDefinition();
        }

        public Type GenericTypeDefinition
        {
            get { return GetGenericTypeDefinition(); }
        }

        public bool IsNullable
        {
            get { return GenericTypeDefinition == typeof(Nullable<>); }
        }

        public Type[] GetGenericArguments()
        {
            if (innerType == null)
                return new Type[0];

            return innerType.GetGenericArguments();
        }

        public IEnumerable<Type> GenericArguments
        {
            get { return GetGenericArguments(); }
        }

        public override string ToString()
        {
            return innerType == null ? innerName : innerType.AssemblyQualifiedName;
        }

        public bool Equals(TypeReference other)
        {
            if (other.innerType == null && innerType == null)
                return Equals(other.innerName, innerName);
            
            return Equals(other.innerType, innerType);
        }

        public bool Equals(Type other)
        {
            return Equals(other, innerType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() == typeof(TypeReference))
                return Equals((TypeReference)obj);
            if (obj.GetType() == typeof(Type))
                return Equals((Type)obj);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((innerType != null ? innerType.GetHashCode() : 0) * 397) ^ (innerName != null ? innerName.GetHashCode() : 0);
            }
        }

        public static implicit operator Type(TypeReference type)
        {
            return type.innerType;
        }

        public static bool operator ==(TypeReference original, Type type)
        {
            if (original.innerType == null)
                return false;

            return original.innerType == type;
        }

        public static bool operator !=(TypeReference original, Type type)
        {
            return !(original == type);
        }
    }
}