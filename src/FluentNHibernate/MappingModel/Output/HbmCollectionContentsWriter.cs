using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionContentsWriter : NullMappingModelVisitor, IXmlWriter<ICollectionContentsMapping>
    {
        private readonly IXmlWriter<OneToManyMapping> _oneToManyWriter;
        private readonly IXmlWriter<ManyToManyMapping> _manyToManyWriter;

        private object _hbm;

        public HbmCollectionContentsWriter(IXmlWriter<OneToManyMapping> oneToManyWriter, IXmlWriter<ManyToManyMapping> manyToManyWriter)
        {
            _oneToManyWriter = oneToManyWriter;
            _manyToManyWriter = manyToManyWriter;
        }

        public object Write(ICollectionContentsMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            _hbm = _oneToManyWriter.Write(oneToManyMapping);
        }

        public override void ProcessManyToMany(ManyToManyMapping manyToManyMapping)
        {
            _hbm = _manyToManyWriter.Write(manyToManyMapping);
        }
    }
}
