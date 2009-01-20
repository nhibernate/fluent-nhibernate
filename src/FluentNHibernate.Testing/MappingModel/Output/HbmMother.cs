using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MappingModel.Output;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    public static class HbmMother
    {        
        public static IHbmWriter<ICollectionContentsMapping> CreateCollectionContentsWriter()
        {
            return new HbmCollectionContentsWriter(CreateOneToManyWriter());
        }

        public static IHbmWriter<OneToManyMapping> CreateOneToManyWriter()
        {
            return new HbmOneToManyWriter();
        }

        public static IHbmWriter<KeyMapping> CreateKeyWriter()
        {
            return new HbmKeyWriter();  
        }

        public static IHbmWriter<IIdentityMapping> CreateIdentityWriter()
        {
            return new HbmIdentityWriter(CreateIdWriter(), new HbmCompositeIdWriter());
        }

        public static IHbmWriter<IdMapping> CreateIdWriter()
        {
            return new HbmIdWriter(CreateColumnWriter(), CreateIdGeneratorWriter());
        }

        public static IHbmWriter<IdGeneratorMapping> CreateIdGeneratorWriter()
        {
            return new HbmIdGeneratorWriter();
        }

        public static IHbmWriter<ColumnMapping> CreateColumnWriter()
        {
            return new HbmColumnWriter();
        }
    }
}
