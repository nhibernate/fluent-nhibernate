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
            return new ClassMapping("class1", CreateNativeIDMapping());    
        }

        public static IdMapping CreateNativeIDMapping()
        {
                return new IdMapping("ID",
                    new IdColumnMapping("TestID"),
                    IdGeneratorMapping.NativeGenerator
                    );
        }
    }
}
