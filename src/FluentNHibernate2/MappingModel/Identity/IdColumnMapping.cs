using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdColumnMapping : MappingBase<HbmColumn>
    {
        public IdColumnMapping(string name)
        {
            Name = name;
        }

        public string Name
        {
            get { return _hbm.name; }
            set { _hbm.name = value; }
        }
    }
}