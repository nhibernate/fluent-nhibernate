using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IManyToManyInstance : IManyToManyInspector, IRelationshipInstance
    {
        void Column(string columnName);
        new IEnumerable<IColumnInstance> Columns { get; }
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