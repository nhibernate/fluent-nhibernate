using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionContentsWriter : MappingModelVisitorBase, IHbmWriter<ICollectionContentsMapping>
    {
        private readonly IHbmWriter<OneToManyMapping> _oneToManyWriter;

        private object _hbm;

        public HbmCollectionContentsWriter(IHbmWriter<OneToManyMapping> oneToManyWriter)
        {
            _oneToManyWriter = oneToManyWriter;
        }

        public object Write(ICollectionContentsMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            _hbm = _oneToManyWriter.Write(oneToManyMapping);
        }
    }
}
