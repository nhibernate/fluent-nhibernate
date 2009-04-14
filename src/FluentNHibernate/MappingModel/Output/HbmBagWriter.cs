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
    public class HbmBagWriter : NullMappingModelVisitor, IXmlWriter<BagMapping>
    {
        private readonly IXmlWriter<ICollectionContentsMapping> _contentsWriter;
        private readonly IXmlWriter<KeyMapping> _keyWriter;

        private HbmBag _hbm;

        public HbmBagWriter(IXmlWriter<ICollectionContentsMapping> contentsWriter, 
            IXmlWriter<KeyMapping> keyWriter)
        {
            _contentsWriter = contentsWriter;
            _keyWriter = keyWriter;
        }

        public object Write(BagMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessBag(BagMapping bagMapping)
        {
            _hbm = new HbmBag();
            _hbm.name = bagMapping.Name;

            if(bagMapping.Attributes.IsSpecified(x => x.OrderBy))
                _hbm.orderby = bagMapping.OrderBy;

            if (bagMapping.Attributes.IsSpecified(x => x.IsInverse))
                _hbm.inverse = bagMapping.IsInverse;

            if( bagMapping.Attributes.IsSpecified(x => x.IsLazy))
            {
                _hbm.SetLazy(bagMapping.IsLazy);
            }
        }

        public override void Visit(ICollectionContentsMapping contentsMapping)
        {
            object contentsHbm = _contentsWriter.Write(contentsMapping);
            _hbm.SetContents(contentsHbm);
        }

        public override void Visit(KeyMapping keyMapping)
        {
            HbmKey keyHbm = (HbmKey)_keyWriter.Write(keyMapping);
            _hbm.key = keyHbm;
        }
    }

}
