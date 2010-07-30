namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    class TwoPropertyEntity
    {
        public int Id { get; set; }
        public string TargetProperty { get; set; }
        public string OtherProperty { get; set; }
    }
}