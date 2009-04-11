using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Output
{
    internal class HbmMappedMemberWriterHelper
    {
        private readonly IHbmWriter<ICollectionMapping> _collectionWriter;
        private readonly IHbmWriter<PropertyMapping> _propertyWriter;
        private readonly IHbmWriter<ManyToOneMapping> _manyToOneWriter;
        private readonly IHbmWriter<ComponentMapping> _componentWriter;

        public HbmMappedMemberWriterHelper(IHbmWriter<ICollectionMapping> collectionWriter, IHbmWriter<PropertyMapping> propertyWriter, IHbmWriter<ManyToOneMapping> manyToOneWriter, IHbmWriter<ComponentMapping> componentWriter)
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
