using System.Xml;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.QuickStart.Domain.Mapping
{
    public class CatMap : ClassMap<Cat>, IMapGenerator
    {
        public CatMap()
        {
            //set up our generator as UUID.HEX
            Id(x => x.Id)
                .GeneratedBy
                .UuidHex("B");


            //non-nullable string with a length of 16
            Map(x => x.Name)
                .WithLengthOf(16)
                .Not.Nullable();

            //simple properties
            Map(x => x.Sex);
            Map(x => x.Weight);
        }

        #region IMapGenerator Members

        public XmlDocument Generate()
        {
            return CreateMapping(new MappingVisitor());
        }

        #endregion
    }
}