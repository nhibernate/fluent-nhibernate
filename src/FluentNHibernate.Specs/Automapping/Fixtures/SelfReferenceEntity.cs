namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public class SelfReferenceEntity
    {
        public int Id { get; set; }
        public SelfReferenceEntity Parent { get; set; }
    }
}