using System;

namespace FluentNHibernate.Mapping;

public class NotFoundExpression<TParent>(TParent parent, Action<string> setter)
{
    /// <summary>
    /// Used to set the Not-Found attribute to ignore.  This tells NHibernate to 
    /// return a null object rather then throw an exception when the join fails
    /// </summary>
    public TParent Ignore()
    {
        setter("ignore");
        return parent;
    }

    /// <summary>
    /// Used to set the Not-Found attribute to exception (Nhibernate default).  This 
    /// tells NHibernate to throw an exception when the join fails
    /// </summary>
    public TParent Exception()
    {
        setter("exception");
        return parent;
    }
}
