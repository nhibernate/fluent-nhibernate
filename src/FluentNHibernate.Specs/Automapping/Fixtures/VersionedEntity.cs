namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public class VersionedEntity
    {
        public int Id { get; set; }
        public int Timestamp { get; set; }
        public string AnUnobviousVersion { get; set; }
    }
}