using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmKeyWriter : MappingModelVisitorBase, IHbmWriter<KeyMapping>
    {
        private HbmKey _hbmKey;

        public object Write(KeyMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbmKey;
        }

        public override void ProcessKey(KeyMapping keyMapping)
        {
            _hbmKey = new HbmKey();

            if (keyMapping.Attributes.IsSpecified(x => x.Column))
                _hbmKey.column1 = keyMapping.Column;

            if (keyMapping.Attributes.IsSpecified(x => x.ForeignKey))
                _hbmKey.foreignkey = keyMapping.ForeignKey;

            if (keyMapping.Attributes.IsSpecified(x => x.PropertyReference))
                _hbmKey.propertyref = keyMapping.PropertyReference;

            if (keyMapping.Attributes.IsSpecified(x => x.CascadeOnDelete))
            {
                _hbmKey.SetCascadeOnDelete(keyMapping.CascadeOnDelete);
            }
        }
    }
}
