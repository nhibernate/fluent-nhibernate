using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IManyToManyInspector : IRelationshipInspector
    {
        IDefaultableEnumerable<IColumnInspector> Columns { get; }
        Type ChildType { get; }
        Fetch Fetch { get; }
        string ForeignKey { get; }
        bool LazyLoad { get; }
        NotFound NotFound { get; }
        Type ParentType { get; }

        /// <summary>
        /// Applies to the joining table for this many-to-many. 
        /// </summary>
        string Where { get; }

        /// <summary>
        /// Applies to the joining table for this many-to-many. 
        /// </summary>
        string OrderBy { get; }
    }
}