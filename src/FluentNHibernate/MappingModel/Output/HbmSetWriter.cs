using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmSetWriter : NullMappingModelVisitor, IXmlWriter<SetMapping>
    {
        private readonly IXmlWriter<ICollectionContentsMapping> _contentsWriter;
        private readonly IXmlWriter<KeyMapping> _keyWriter;

        private HbmSet _hbm;

        public HbmSetWriter(IXmlWriter<ICollectionContentsMapping> contentsWriter, IXmlWriter<KeyMapping> keyWriter)
        {
            _contentsWriter = contentsWriter;
            _keyWriter = keyWriter;
        }

        public object Write(SetMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessSet(SetMapping setMapping)
        {
            _hbm = new HbmSet();
            _hbm.name = setMapping.Name;

            if(setMapping.Attributes.IsSpecified(x => x.OrderBy))
                _hbm.orderby = setMapping.OrderBy;

            if (setMapping.Attributes.IsSpecified(x => x.IsInverse))
                _hbm.inverse = setMapping.IsInverse;

            if (setMapping.Attributes.IsSpecified(x => x.IsLazy))
            {
                _hbm.SetLazy(setMapping.IsLazy);
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
