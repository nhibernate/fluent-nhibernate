namespace FluentNHibernate.Specs.FluentInterface.Fixtures;

class EntityWithProperties
{
    public string Name { get; set; }
}

class EntityWithPrivateProperties
{
    string Name { get; set; }
    string name;
}
