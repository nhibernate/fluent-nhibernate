namespace FluentNHibernate.MappingModel
{
    public class VersionGenerated
    {
        public static readonly VersionGenerated Never = new VersionGenerated("never");
        public static readonly VersionGenerated Always = new VersionGenerated("always");

        private readonly string value;

        private VersionGenerated(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}