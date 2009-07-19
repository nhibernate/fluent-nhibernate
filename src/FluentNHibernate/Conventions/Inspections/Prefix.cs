namespace FluentNHibernate.Conventions.Inspections
{
    public class Prefix
    {
        private readonly string value;

        protected Prefix(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }    
    }
}