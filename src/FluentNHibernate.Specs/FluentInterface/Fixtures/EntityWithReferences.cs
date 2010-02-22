namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithReferences
    {
        public ReferenceTarget Reference { get; set; }
    }

    class ReferenceTarget
    {}
}