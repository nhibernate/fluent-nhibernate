using System.Linq;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmStoredProcedureConverter : HbmConverterBase<StoredProcedureMapping, HbmCustomSQL>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCustomSQLCheck> checkDict = new XmlLinkedEnumBiDictionary<HbmCustomSQLCheck>();

        private HbmCustomSQL hbmCustomSQL;

        public HbmStoredProcedureConverter() : base(null)
        {
        }

        public override HbmCustomSQL Convert(StoredProcedureMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmCustomSQL;
        }

        public override void ProcessStoredProcedure(StoredProcedureMapping storedProcedureMapping)
        {
            hbmCustomSQL = new HbmCustomSQL();

            // Realistically, this is always considered specified, but this is arguably more correct if that ever changes
            bool checkSpecified = storedProcedureMapping.IsSpecified("Check");
            hbmCustomSQL.checkSpecified = checkSpecified;
            if (checkSpecified)
                hbmCustomSQL.check = LookupEnumValueIn(checkDict, storedProcedureMapping.Check);

            hbmCustomSQL.Text = new string[] { storedProcedureMapping.Query };
        }
    }
}
