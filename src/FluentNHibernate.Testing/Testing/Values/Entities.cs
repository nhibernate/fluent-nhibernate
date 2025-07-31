using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentNHibernate.Testing.Testing.Values;

public class ListEntity
{
    readonly IList<string> backingField = new List<string>();

    //Set = new HashedSet();

    public IEnumerable<string> GetterAndSetter { get; set; } = new List<string>();
    public IEnumerable<string> GetterAndPrivateSetter { get; private set; } = new List<string>();
    public IEnumerable<string> BackingField => backingField;

    public ISet<string> TypedSet { get; set; } = new HashSet<string>();

    //public ISet Set { get; set; }
    public ICollection Collection { get; set; } = new StringCollection();
    public string[] Array { get; set; }
    public IList<string> List { get; set; } = new List<string>();

    public void AddListItem(string value)
    {
        backingField.Add(value);
    }
}

public class ReferenceEntity
{
    public OtherEntity Reference { get; set; }
    public IEnumerable<OtherEntity> ReferenceList { get; set; } = new List<OtherEntity>();

    public void SetReference(OtherEntity value)
    {
        Reference = value;
    }
}

public class PropertyEntity
{
    string backingField;

    public string GetterAndSetter { get; set; }
    public string GetterAndPrivateSetter { get; private set; }

    public string BackingField => backingField;

    public void SetBackingField(string value)
    {
        backingField = value;
    }
}

public class OtherEntity
{}
