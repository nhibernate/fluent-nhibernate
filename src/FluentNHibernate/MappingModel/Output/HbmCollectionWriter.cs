using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionWriter : NullMappingModelVisitor, IXmlWriter<ICollectionMapping>
    {
        private readonly IXmlWriter<BagMapping> _bagWriter;
        private readonly IXmlWriter<SetMapping> _setWriter;

        private object _hbm;

        public HbmCollectionWriter(IXmlWriter<BagMapping> bagWriter, IXmlWriter<SetMapping> setWriter)
        {
            _bagWriter = bagWriter;
            _setWriter = setWriter;
        }

        public object Write(ICollectionMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessBag(BagMapping bagMapping)
        {
            _hbm = _bagWriter.Write(bagMapping);
        }

        public override void ProcessSet(SetMapping setMapping)
        {
            _hbm = _setWriter.Write(setMapping);
        }


    }
}