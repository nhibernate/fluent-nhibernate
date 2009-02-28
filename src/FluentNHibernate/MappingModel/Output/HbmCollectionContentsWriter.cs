using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionContentsWriter : NullMappingModelVisitor, IHbmWriter<ICollectionContentsMapping>
    {
        private readonly IHbmWriter<OneToManyMapping> _oneToManyWriter;
        private readonly IHbmWriter<ManyToManyMapping> _manyToManyWriter;

        private object _hbm;

        public HbmCollectionContentsWriter(IHbmWriter<OneToManyMapping> oneToManyWriter, IHbmWriter<ManyToManyMapping> manyToManyWriter)
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
