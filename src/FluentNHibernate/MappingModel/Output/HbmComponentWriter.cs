using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmComponentWriter : NullMappingModelVisitor, IXmlWriter<ComponentMapping>
    {
        private readonly HbmMappedMemberWriterHelper _mappedMemberHelper;
        private HbmComponent _hbm;

        private HbmComponentWriter(HbmMappedMemberWriterHelper mappedMemberHelper)
        {
            _mappedMemberHelper = mappedMemberHelper;
        }

        public HbmComponentWriter(IXmlWriter<IIdentityMapping> identityWriter, IXmlWriter<ICollectionMapping> collectionWriter, IXmlWriter<PropertyMapping> propertyWriter, IXmlWriter<ManyToOneMapping> manyToOneWriter)
            : this(new HbmMappedMemberWriterHelper(collectionWriter, propertyWriter, manyToOneWriter, null))
        {
        }

        public object Write(ComponentMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessComponent(ComponentMapping componentMapping)
        {
            _hbm = new HbmComponent();
            _hbm.name = componentMapping.PropertyName;

            if(componentMapping.Attributes.IsSpecified(x => x.ClassName))
                _hbm.@class = componentMapping.ClassName;

        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            object hbmProperty = _mappedMemberHelper.Write(propertyMapping);
            hbmProperty.AddTo(ref _hbm.Items);
        }

        public override void Visit(ICollectionMapping collectionMapping)
        {
            object hbmCollection = _mappedMemberHelper.Write(collectionMapping);
            hbmCollection.AddTo(ref _hbm.Items);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            object hbmManyToOne = _mappedMemberHelper.Write(manyToOneMapping);
            hbmManyToOne.AddTo(ref _hbm.Items);
        }

        public override void Visit(ComponentMapping componentMapping)
        {
            var componentWriter = new HbmComponentWriter(_mappedMemberHelper);
            object hbmComponent = componentWriter.Write(componentMapping);
            hbmComponent.AddTo(ref _hbm.Items);
        }
    }
}
