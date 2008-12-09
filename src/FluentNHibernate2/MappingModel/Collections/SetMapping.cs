using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Collections
{
    public class SetMapping : CollectionMappingBase<HbmSet>
    {
        public SetMapping()
        {
            
        }

        public SetMapping(string name, KeyMapping key, ICollectionContentsMapping contents)
        {
            Name = name;
            Key = key;
            Contents = contents;
        }

        public override string Name
        {
            get { return _hbm.name; }
            set { _hbm.name = value; }
        }

        public override KeyMapping Key
        {
            get { return _key; }
            set
            {
                _key = value;
                _hbm.key = _key.Hbm;
            }
        }

        public override ICollectionContentsMapping Contents
        {
            get { return _contents; }
            set
            {
                _contents = value;
                _hbm.Item1 = _contents.Hbm;
            }
        }
    }
}