using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class TypeReference
    {
        public static readonly TypeReference Empty = new TypeReference("nop");

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

                if (innerType.IsGenericType 
                    && innerType.GetGenericTypeDefinition() == typeof(FluentNHibernate.Mapping.GenericEnumMapper<>))
                {
                    return true;
                }
                

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
            if(ReferenceEquals(other, null))
                return false;
            if (other.innerType == null && innerType == null)
                return other.innerName.Equals(innerName);                        
            if (other.innerType != null)
                return other.innerType.Equals(innerType);

            return false;
        }

        public bool Equals(Type other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return other.Equals(innerType);
        }

        public bool Equals(string other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return other.Equals(innerName);
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;
            if (obj.GetType() == typeof(TypeReference))
                return Equals((TypeReference)obj);
            if (obj is Type)
                return Equals((Type)obj);
            if (obj.GetType() == typeof(string))
                return Equals((string)obj);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((innerType != null ? innerType.GetHashCode() : 0) * 397) ^ (innerName != null ? innerName.GetHashCode() : 0);
            }
        }

        public Type GetUnderlyingSystemType()
        {
            return innerType;
        }

        public static bool operator ==(TypeReference original, Type type)
        {
            if (type == null)
                return false;
            if (original == (Type)null || original.innerType == null)
                return false;

            return original.innerType == type;
        }

        public static bool operator !=(TypeReference original, Type type)
        {
            return !(original == type);
        }

        public static bool operator ==(Type original, TypeReference type)
        {
            return type == original;
        }

        public static bool operator !=(Type original, TypeReference type)
        {
            return !(original == type);
        }

        public static bool operator ==(TypeReference original, TypeReference other)
        {
            return original.Equals(other);
        }

        public static bool operator !=(TypeReference original, TypeReference other)
        {
            return !(original == other);
        }
    }
}