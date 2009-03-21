using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmIndexWriter : NullMappingModelVisitor, IHbmWriter<IndexMapping>
    {
        private HbmIndex _hbm;

        public object Write(IndexMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessIndex(IndexMapping indexMapping)
        {
            _hbm = new HbmIndex();

            if (indexMapping.Attributes.IsSpecified(x => x.Column))
                _hbm.column1 = indexMapping.Column;

            if (indexMapping.Attributes.IsSpecified(x => x.IndexType))
                _hbm.type = indexMapping.IndexType;

            if (indexMapping.Attributes.IsSpecified(x => x.Length))
                _hbm.length = indexMapping.Length.ToString();
        }
        
    }
}
