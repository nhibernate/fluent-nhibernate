namespace FluentNHibernate.Specs.Utilities.Fixtures
{
    public class Target : TargetParent
    {
        private int IntProperty { get; set; }
        private string PrivateProperty { get; set; }
        private string privateField;
        protected string ProtectedProperty { get; set; }
        protected string protectedField;
        public string PublicProperty { get; set; }
        public string publicField;
    }
}