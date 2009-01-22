using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmBagWriter : MappingModelVisitorBase, IHbmWriter<BagMapping>
    {
        private readonly IHbmWriter<ICollectionContentsMapping> _contentsWriter;
        private readonly IHbmWriter<KeyMapping> _keyWriter;

        private HbmBag _hbmBag;

        public HbmBagWriter(IHbmWriter<ICollectionContentsMapping> contentsWriter, 
            IHbmWriter<KeyMapping> keyWriter)
        {
            _contentsWriter = contentsWriter;
            _keyWriter = keyWriter;
        }

        public object Write(BagMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmBag;
        }

        public override void ProcessBag(BagMapping bagMapping)
        {
            _hbmBag = new HbmBag();
            _hbmBag.name = bagMapping.Name;

            if(bagMapping.Attributes.IsSpecified(x => x.OrderBy))
                _hbmBag.orderby = bagMapping.OrderBy;

            if (bagMapping.Attributes.IsSpecified(x => x.IsInverse))
                _hbmBag.inverse = bagMapping.IsInverse;

            if( bagMapping.Attributes.IsSpecified(x => x.IsLazy))
            {
                _hbmBag.SetLazy(bagMapping.IsLazy);
            }
        }

        public override void ProcessCollectionContents(ICollectionContentsMapping contentsMapping)
        {
            object contentsHbm = _contentsWriter.Write(contentsMapping);
            _hbmBag.SetContents(contentsHbm);
        }

        public override void ProcessKey(KeyMapping keyMapping)
        {
            HbmKey keyHbm = (HbmKey)_keyWriter.Write(keyMapping);
            _hbmBag.key = keyHbm;
        }
    }

}
