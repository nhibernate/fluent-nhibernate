namespace FluentNHibernate.Specs.Utilities.Fixtures;

public class Target : TargetParent
{
    int IntProperty { get; set; }
    string PrivateProperty { get; set; }
    string privateField;
    protected string ProtectedProperty { get; set; }
    protected string protectedField;
    public string PublicProperty { get; set; }
    public string publicField;
}
