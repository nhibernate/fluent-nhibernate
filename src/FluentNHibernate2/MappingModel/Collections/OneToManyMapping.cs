using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Collections
{
    public class OneToManyMapping : MappingBase<HbmOneToMany>, ICollectionContentsMapping
    {
        public OneToManyMapping(string className)
        {
            ClassName = className;
        }

        public string ClassName
        {
            get { return _hbm.@class; }
            set { _hbm.@class = value; }
        }
    }
}