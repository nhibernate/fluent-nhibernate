using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToManyInstance : IManyToManyInspector, IRelationshipInstance
    {
        void Column(string columnName);
        new IDefaultableEnumerable<IColumnInstance> Columns { get; }
        new void ForeignKey(string constraint);

        /// <summary>
        /// Applies to the joining table for this many-to-many. 
        /// </summary>
        new void Where(string where);

        /// <summary>
        /// Applies to the joining table for this many-to-many. 
        /// </summary>
        new void OrderBy(string orderBy);
    }
}