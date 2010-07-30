using System;

namespace FluentNHibernate.MappingModel.ClassBased
{
    [Serializable]
    public class SubclassType
    {
        public static readonly SubclassType Subclass = new SubclassType("subclass");
        public static readonly SubclassType JoinedSubclass = new SubclassType("joined-subclass");
        public static readonly SubclassType UnionSubclass = new SubclassType("union-subclass");

        readonly string elementName;

        private SubclassType(string elementName)
        {
            this.elementName = elementName;
        }

        public string GetElementName()
        {
            return elementName;
        }

        public bool Equals(SubclassType other)
        {
            return Equals(other.elementName, elementName);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SubclassType)) return false;
            return Equals((SubclassType)obj);
        }

        public override int GetHashCode()
        {
            return (elementName != null ? elementName.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Format("ElementName: {0}", elementName);
        }

        public static bool operator ==(SubclassType left, SubclassType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SubclassType left, SubclassType right)
        {
            return !(left == right);
        }
    }
}