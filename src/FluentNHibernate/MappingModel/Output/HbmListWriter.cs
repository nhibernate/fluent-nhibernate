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
    public class HbmListWriter : NullMappingModelVisitor, IHbmWriter<ListMapping>
    {
        private readonly IHbmWriter<ICollectionContentsMapping> _contentsWriter;
        private readonly IHbmWriter<KeyMapping> _keyWriter;

        private HbmList _hbm;
        private IHbmWriter<IndexMapping> _indexWriter;

        public HbmListWriter(IHbmWriter<ICollectionContentsMapping> contentsWriter,
            IHbmWriter<KeyMapping> keyWriter, IHbmWriter<IndexMapping> indexWriter)
        {
            _contentsWriter = contentsWriter;
            _indexWriter = indexWriter;
            _keyWriter = keyWriter;
        }

        public object Write(ListMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessList(ListMapping listMapping)
        {
            _hbm = new HbmList();
            _hbm.name = listMapping.Name;

            if (listMapping.Attributes.IsSpecified(x => x.IsInverse))
                _hbm.inverse = listMapping.IsInverse;

            if (listMapping.Attributes.IsSpecified(x => x.IsLazy))
                _hbm.SetLazy(listMapping.IsLazy);
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

        public override void Visit(IndexMapping indexMapping)
        {
            HbmIndex indexHbm = (HbmIndex) _indexWriter.Write(indexMapping);
            _hbm.SetIndex(indexHbm);
        }
    }

}
