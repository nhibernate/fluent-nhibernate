namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Specifies the position within the parent that this element is output
    /// </summary>
    public enum PartPosition
    {
        Anywhere = 0, //Zero makes it the default if not explicitly chosen
        First = -1, //Negative One means when compared to as an integer this should appear first, before even the default value
        Last = 1 //One is the highest number here, and should allow Parts with this specification to sort last
    }

    //Order of elements (using Level value) that go first during ClassMap is as follows:
    //<hibernate-mapping> -2
    //<import> -1
    //<class> 0
    //<cache> 1
    //<id> 2
    //<composite-id> 2 (id and compositeId are mutually exclusive and exist in the same mapping order due to this)
    //<discriminator> 3
    //<version> 4
}