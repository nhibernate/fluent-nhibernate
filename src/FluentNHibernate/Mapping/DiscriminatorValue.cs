namespace FluentNHibernate.Mapping;

/// <summary>
/// Pre-defined discriminator values
/// </summary>
public class DiscriminatorValue
{
    /// <summary>
    /// Null discriminator value
    /// </summary>
    public static readonly DiscriminatorValue Null = new DiscriminatorValue("null");
        
    /// <summary>
    /// Non-null discriminator value
    /// </summary>
    public static readonly DiscriminatorValue NotNull = new DiscriminatorValue("not null");

    readonly string outputValue;

    DiscriminatorValue(string outputValue)
    {
        this.outputValue = outputValue;
    }

    public override string ToString()
    {
        return outputValue;
    }
}
