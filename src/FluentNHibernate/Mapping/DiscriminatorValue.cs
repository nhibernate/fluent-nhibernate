namespace FluentNHibernate.Mapping
{
    public class DiscriminatorValue
    {
        public static readonly DiscriminatorValue Null = new DiscriminatorValue("null");
        public static readonly DiscriminatorValue NotNull = new DiscriminatorValue("not null");

        private readonly string outputValue;

        private DiscriminatorValue(string outputValue)
        {
            this.outputValue = outputValue;
        }

        public override string ToString()
        {
            return outputValue;
        }
    }
}