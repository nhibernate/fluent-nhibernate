namespace FluentNHibernate.Mapping
{
    public interface INotFoundExpression
    {
        /// <summary>
        /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
        /// return a null object rather then throw an exception when the join fails
        /// </summary>
        void Ignore();

        /// <summary>
        /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
        /// tells NHibernate to throw an exception when the join fails
        /// </summary>
        void Exception();
    }
}