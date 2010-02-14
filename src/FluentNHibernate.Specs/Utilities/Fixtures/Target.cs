namespace FluentNHibernate.Specs.Utilities.Fixtures
{
    public class Target : TargetParent
    {
        private int IntProperty { get; set; }
        private string PrivateProperty { get; set; }
        protected string ProtectedProperty { get; set; }
        protected string PublicProperty { get; set; }
    }
}