using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IFilterDefinition
    {
        string Name { get; }
        FilterDefinitionMapping GetFilterMapping();
        HibernateMapping GetHibernateMapping();
    }
}
