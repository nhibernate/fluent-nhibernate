namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithProperties
    {
        public string Name { get; set; }
    }

    class EntityWithPrivateProperties
    {
        private string Name { get; set; }
        string name;
    }
}
