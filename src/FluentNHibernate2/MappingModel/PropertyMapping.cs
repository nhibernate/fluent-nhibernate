using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class PropertyMapping : MappingBase<HbmProperty>
    {
        public PropertyMapping(string name)
        {
            Name = name;
        }

        public string Name
        {
            get { return _hbm.name;  }
            set { _hbm.name = value; }
        }

        public int Length
        {
            get { return Convert.ToInt32(_hbm.length); }
            set { _hbm.length = value.ToString(); }
        }

        public bool AllowNull
        {
            get { return !_hbm.notnull; }
            set { _hbm.notnull = !value; }
        }
    }
}