namespace FluentNHibernate.Conventions.Inspections;

public class Generated
{
    /// <summary>
    /// Use the default value.
    /// </summary>
    public static readonly Generated Unset = new Generated("");

    /// <summary>
    /// The property value as not generated within the database (default).
    /// </summary>
    public static readonly Generated Never = new Generated("never");

    /// <summary>
    /// The property value as generated on INSERT, but is not regenerated on subsequent updates.
    /// <para>NHibernate will immediately issues a SELECT after INSERT to retrieve the generated values.</para>
    /// </summary>
    public static readonly Generated Insert = new Generated("insert");

    /// <summary>
    /// The property value as generated both on INSERT and on UPDATE.
    /// <para>NHibernate will immediately issues a SELECT after INSERT or UPDATE to retrieve the generated values.</para>
    /// </summary>
    public static readonly Generated Always = new Generated("always");

    private readonly string value;

    private Generated(string value)
    {
        this.value = value;
    }

    public override bool Equals(object obj)
    {
        if (obj is Generated) return Equals((Generated) obj);

        return base.Equals(obj);
    }

    public bool Equals(Generated other)
    {
        return Equals(other.value, value);
    }

    public override int GetHashCode()
    {
        return (value != null ? value.GetHashCode() : 0);
    }

    public static bool operator ==(Generated x, Generated y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(Generated x, Generated y)
    {
        return !(x == y);
    }

    public override string ToString()
    {
        return value;
    }

    public static Generated FromString(string value)
    {
        return new Generated(value);
    }
}
