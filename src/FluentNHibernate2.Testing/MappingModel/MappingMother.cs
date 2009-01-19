using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Testing.MappingModel
{
    public static class MappingMother
    {
        public static ClassMapping CreateClassMapping()
        {
            return new ClassMapping {Name = "class1" };
        }

        public static IdMapping CreateNativeIDMapping()
        {
            return new IdMapping(new ColumnMapping { Name = "TestID" })
            {
                Name = "ID",
                    Generator = IdGeneratorMapping.NativeGenerator
            };
        }
    }
}
