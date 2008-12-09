using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ManyToOneMapping : MappingBase<HbmManyToOne>
    {
        public ManyToOneMapping(string name)
        {
            Name = name;
        }

        public string Name
        {
            get { return _hbm.name; }
            set { _hbm.name = value; }
        }

        public bool AllowNull
        {
            get { return !_hbm.notnull; }
            set { _hbm.notnull = !value; }
        }
    }
}