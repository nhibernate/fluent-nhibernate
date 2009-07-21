using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IIndexInspector : IInspector
    {
        TypeReference Type { get; }
        IEnumerable<IColumnInspector> Columns { get; }
    }
}
