namespace FluentNHibernate.MappingModel.ClassBased
{
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
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.elementName, elementName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
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
    }
}