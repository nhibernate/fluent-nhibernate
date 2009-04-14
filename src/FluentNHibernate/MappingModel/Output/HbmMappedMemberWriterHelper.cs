using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    internal class HbmMappedMemberWriterHelper
    {
        private readonly IXmlWriter<ICollectionMapping> _collectionWriter;
        private readonly IXmlWriter<PropertyMapping> _propertyWriter;
        private readonly IXmlWriter<ManyToOneMapping> _manyToOneWriter;
        private readonly IXmlWriter<ComponentMapping> _componentWriter;

        public HbmMappedMemberWriterHelper(IXmlWriter<ICollectionMapping> collectionWriter, IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ManyToOneMapping> manyToOneWriter, IXmlWriter<ComponentMapping> componentWriter)
        {
            _collectionWriter = collectionWriter;
            _componentWriter = componentWriter;
            _manyToOneWriter = manyToOneWriter;
            _propertyWriter = propertyWriter;
        }

        public object Write(PropertyMapping mappingModel)
        {
            return _propertyWriter.Write(mappingModel);
        }

        public object Write(ManyToOneMapping mappingModel)
        {
            return _manyToOneWriter.Write(mappingModel);
        }

        public object Write(ComponentMapping mappingModel)
        {
            return _componentWriter.Write(mappingModel);
        }

        public object Write(ICollectionMapping mappingModel)
        {
            return _collectionWriter.Write(mappingModel);
        }
    }
}
