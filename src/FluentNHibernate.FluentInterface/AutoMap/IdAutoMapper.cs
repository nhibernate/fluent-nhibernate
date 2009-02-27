using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.FluentInterface.AutoMap
{
    public class IdAutoMapper : IAutoMapper
    {
        public void Map(ClassMapping classMap)
        {
            var idProperty = classMap.Type.GetProperty("Id");
            var columnMapping = new ColumnMapping { Name = "Id", PropertyInfo = idProperty };
            var mapping = new IdMapping(columnMapping)
                              {
                                  Name = "Id",
                                  PropertyInfo = idProperty,
                                  Generator = new IdGeneratorMapping()
                              };
            classMap.Id = mapping;
        }
    }
}