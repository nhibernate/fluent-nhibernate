namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Specifies the position within the parent that this element is output
    /// </summary>
    public enum PartPosition
    {
        Anywhere,
        First,
        Last
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