using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.FluentInterface.AutoMap
{
    public interface IAutoMapper {
        void Map(ClassMapping classMap);
    }
}