namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public class EntityWithStaticProperties
    {
        public int Id { get; set; }
        public static int StaticProperty { get; set; }
    }
}