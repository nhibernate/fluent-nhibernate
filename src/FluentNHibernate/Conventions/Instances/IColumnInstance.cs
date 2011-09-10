using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IColumnInstance : IColumnInspector
    {
        new void Length(int length);
        new void Index(string indexname);
        new void Default(string defaultvalue);
    }
}