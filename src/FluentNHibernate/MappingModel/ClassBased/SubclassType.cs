namespace FluentNHibernate.MappingModel.ClassBased
{
    public class SubclassType
    {
        public static readonly SubclassType Subclass = new SubclassType("subclass");
        public static readonly SubclassType JoinedSubclass = new SubclassType("joined-subclass");

        readonly string elementName;

        private SubclassType(string elementName)
        {
            this.elementName = elementName;
        }

        public string GetElementName()
        {
            return elementName;
        }
    }
}