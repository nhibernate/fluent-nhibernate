using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionWriter : MappingModelVisitorBase, IHbmWriter<ICollectionMapping>
    {
        private readonly IHbmWriter<BagMapping> _bagWriter;
        private readonly IHbmWriter<SetMapping> _setWriter;

        private object _hbm;

        public HbmCollectionWriter(IHbmWriter<BagMapping> bagWriter, IHbmWriter<SetMapping> setWriter)
        {
            _bagWriter = bagWriter;
            _setWriter = setWriter;
        }

        public object Write(ICollectionMapping mappingModel)
        {
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

        public override void ProcessCollectionContents(ICollectionContentsMapping contentsMapping)
        {
            // NOOP
        }

        public override void ProcessKey(KeyMapping keyMapping)
        {
            // NOOP
        }

    }
}